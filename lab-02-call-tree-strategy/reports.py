"""Три независимые стратегии отчёта: TableReport, TreeReport и JSONReport."""

from __future__ import annotations

import json
from collections import defaultdict
from pathlib import Path
from typing import TYPE_CHECKING, Dict, List

if TYPE_CHECKING:
    from profiler import Node, Profiler


def _header() -> str:
    return f"{'Имя':<27}{'Вызовы':>9}{'Всего, мс':>14}{'Собств., мс':>15}"


def _row(name: str, calls: int, total: float, own: float) -> str:
    return f"{name:<27}{calls:>9}{total * 1000:>14.1f}{own * 1000:>15.1f}"


class TableReport:
    """Плоская таблица: одноимённые узлы разных ветвей объединяются."""

    def render(self, profiler: "Profiler") -> str:
        totals: Dict[str, List[float]] = defaultdict(lambda: [0.0, 0.0, 0.0])

        def visit(node: "Node") -> None:
            record = totals[node.name]
            record[0] += node.calls
            record[1] += node.total
            record[2] += node.self_time
            for child in node.children.values():
                visit(child)

        for root_child in profiler.root.children.values():
            visit(root_child)

        lines = ["TableReport — плоский отчёт", _header(), "-" * 65]
        for name, (calls, total, own) in sorted(
            totals.items(), key=lambda item: (-item[1][1], item[0])
        ):
            lines.append(_row(name, int(calls), total, own))
        return "\n".join(lines)


class TreeReport:
    """Рекурсивный вывод дерева вызовов с отступами по глубине."""

    def render(self, profiler: "Profiler") -> str:
        lines = ["TreeReport — дерево вызовов", _header(), "-" * 65]
        for child in profiler.root.children.values():
            self._walk(child, 0, lines)
        return "\n".join(lines)

    def _walk(self, node: "Node", depth: int, lines: List[str]) -> None:
        lines.append(_row("   " * depth + node.name, node.calls,
                          node.total, node.self_time))
        for child in node.children.values():
            self._walk(child, depth + 1, lines)


class JSONReport:
    """Chrome Trace: каждое событие записывается в JSON для Perfetto."""

    def __init__(self, path: str = "trace.json"):
        self.path = Path(path)

    def render(self, profiler: "Profiler") -> str:
        events = [
            {
                "name": event.name,
                "ph": "X",
                "ts": round(event.start * 1_000_000, 3),
                "dur": round(event.duration * 1_000_000, 3),
                "pid": 1,
                "tid": 1,
            }
            for event in profiler.events
        ]
        # Parent и child могут завершаться в обратном порядке;
        # для удобства просмотра экспортируем по времени начала.
        events.sort(key=lambda entry: (entry["ts"], -entry["dur"]))
        self.path.parent.mkdir(parents=True, exist_ok=True)
        with self.path.open("w", encoding="utf-8") as stream:
            json.dump({"traceEvents": events}, stream, indent=2, ensure_ascii=False)
        return f"Сохранено событий: {len(events)} -> {self.path}"


# Название из лекции (слайд 23); фактически это та же JSON-стратегия.
ChromeTraceExporter = JSONReport
