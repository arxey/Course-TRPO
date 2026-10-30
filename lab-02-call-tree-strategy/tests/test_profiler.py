"""Проверки требований практической работы №2 (без сторонних библиотек)."""

import json
import tempfile
import unittest
from pathlib import Path

from profiler import Profiler
from reports import JSONReport, TableReport, TreeReport


class FakeClock:
    def __init__(self, value=100.0):
        self.value = value

    def __call__(self):
        return self.value

    def advance(self, seconds):
        self.value += seconds


class ProfilerTests(unittest.TestCase):
    def setUp(self):
        self.clock = FakeClock()
        self.p = Profiler(clock=self.clock)

    def test_nested_tree_and_own_time(self):
        with self.p.section("кадр"):
            self.clock.advance(0.001)
            with self.p.section("update"):
                self.clock.advance(0.002)
                with self.p.section("physics"):
                    self.clock.advance(0.003)
                self.clock.advance(0.004)
            self.clock.advance(0.005)
        frame = self.p.root.children["кадр"]
        update = frame.children["update"]
        self.assertAlmostEqual(frame.total, 0.015)
        self.assertAlmostEqual(frame.self_time, 0.006)
        self.assertAlmostEqual(update.total, 0.009)
        self.assertAlmostEqual(update.self_time, 0.006)
        self.assertAlmostEqual(update.children["physics"].total, 0.003)

    def test_calls_aggregate_in_same_parent(self):
        for _ in range(60):
            with self.p.section("кадр"):
                self.clock.advance(0.001)
        frame = self.p.root.children["кадр"]
        self.assertEqual(frame.calls, 60)
        self.assertEqual(len(self.p.root.children), 1)
        self.assertAlmostEqual(frame.total, 0.060)

    def test_identical_names_under_different_parents_have_distinct_nodes(self):
        for parent in ("A", "B"):
            with self.p.section(parent):
                with self.p.section("работа"):
                    self.clock.advance(0.001)
        a = self.p.root.children["A"].children["работа"]
        b = self.p.root.children["B"].children["работа"]
        self.assertIsNot(a, b)
        self.assertIn("работа", self.p.report(TableReport()))

    def test_recursion_with_same_name(self):
        with self.p.section("recursive"):
            with self.p.section("recursive"):
                self.clock.advance(0.001)
        outer = self.p.root.children["recursive"]
        inner = outer.children["recursive"]
        self.assertEqual(outer.calls, 1)
        self.assertEqual(inner.calls, 1)
        self.assertAlmostEqual(outer.self_time, 0.0)

    def test_exception_restores_parent_and_registers_event(self):
        with self.assertRaisesRegex(ValueError, "test"):
            with self.p.section("failure"):
                self.clock.advance(0.007)
                raise ValueError("test")
        self.assertIs(self.p._current, self.p.root)
        self.assertEqual(self.p.root.children["failure"].calls, 1)
        self.assertEqual(len(self.p.events), 1)
        with self.p.section("next"):
            self.clock.advance(0.001)
        self.assertIn("next", self.p.root.children)

    def test_decorator_returns_function_result(self):
        @self.p.profile
        def multiply(x, y):
            self.clock.advance(0.004)
            return x * y
        self.assertEqual(multiply(6, 7), 42)
        self.assertEqual(self.p.root.children["multiply"].calls, 1)
        self.assertAlmostEqual(self.p.root.children["multiply"].total, 0.004)

    def test_tree_report_has_nesting_and_columns(self):
        with self.p.section("кадр"):
            with self.p.section("update"):
                self.clock.advance(0.001)
        text = self.p.report(TreeReport())
        self.assertIn("Собств., мс", text)
        self.assertIn("   update", text)
        self.assertIn("кадр", text)

    def test_table_report_flattens_same_name(self):
        for parent in ("first", "second"):
            with self.p.section(parent):
                with self.p.section("shared"):
                    self.clock.advance(0.002)
        text = self.p.report(TableReport())
        rows = [line for line in text.splitlines() if line.startswith("shared")]
        self.assertEqual(len(rows), 1)
        self.assertIn("2", rows[0])
        self.assertIn("4.0", rows[0])

    def test_json_export_is_perfetto_compatible(self):
        self.clock.advance(0.010)
        with self.p.section("русское имя"):
            self.clock.advance(0.005)
        with tempfile.TemporaryDirectory() as directory:
            filename = Path(directory) / "trace.json"
            message = self.p.report(JSONReport(str(filename)))
            raw = filename.read_text(encoding="utf-8")
            data = json.loads(raw)
        self.assertIn("Сохранено событий: 1", message)
        self.assertIn("русское имя", raw)
        self.assertEqual(len(data["traceEvents"]), 1)
        event = data["traceEvents"][0]
        self.assertEqual(event["ph"], "X")
        self.assertEqual(event["pid"], 1)
        self.assertEqual(event["tid"], 1)
        self.assertAlmostEqual(event["ts"], 10_000)
        self.assertAlmostEqual(event["dur"], 5_000)

    def test_events_are_separate_from_aggregated_nodes(self):
        for _ in range(3):
            with self.p.section("job"):
                self.clock.advance(0.001)
        self.assertEqual(self.p.root.children["job"].calls, 3)
        self.assertEqual(len(self.p.events), 3)

    def test_self_time_never_greater_than_total(self):
        with self.p.section("root_child"):
            with self.p.section("child"):
                self.clock.advance(0.2)
        for node in (self.p.root.children["root_child"],
                     self.p.root.children["root_child"].children["child"]):
            self.assertGreaterEqual(node.self_time, 0)
            self.assertLessEqual(node.self_time, node.total)

    def test_no_formatted_report_logic_in_profiler(self):
        class CustomReport:
            def render(self, profiler):
                return "my own format"
        self.assertEqual(self.p.report(CustomReport()), "my own format")

    def test_empty_name_is_rejected(self):
        with self.assertRaises(ValueError):
            with self.p.section(""):
                pass


if __name__ == "__main__":
    unittest.main()
