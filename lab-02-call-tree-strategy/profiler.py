"""Профилировщик со вложенными замерами времени (практическая работа №2).

Реализация рассчитана на однопоточное выполнение, как в задании.
"""

from __future__ import annotations

import time
from contextlib import contextmanager
from dataclasses import dataclass, field
from functools import wraps
from typing import Callable, Dict, Iterator, List, Optional, Protocol, TypeVar


@dataclass
class Node:
    """Узел дерева вызовов, агрегирующий одноимённые вызовы у одного родителя."""

    name: str
    parent: Optional["Node"] = field(default=None, repr=False)
    children: Dict[str, "Node"] = field(default_factory=dict)
    calls: int = 0
    total: float = 0.0  # Секунды, включая время дочерних вызовов.

    def child(self, name: str) -> "Node":
        if name not in self.children:
            self.children[name] = Node(name=name, parent=self)
        return self.children[name]

    @property
    def self_time(self) -> float:
        """Собственное (exclusive) время в секундах."""
        return max(0.0, self.total - sum(c.total for c in self.children.values()))


@dataclass(frozen=True)
class TraceEvent:
    """Один фактический вызов для временной шкалы (не агрегирован)."""

    name: str
    start: float  # Секунды относительно создания Profiler.
    duration: float  # Секунды.


class ReportFormat(Protocol):
    """Контракт стратегии представления результатов профилирования."""

    def render(self, profiler: "Profiler") -> str:
        ...


F = TypeVar("F", bound=Callable)


class Profiler:
    """Собирает замеры, дерево и события; не зависит от форматов отчёта."""

    def __init__(self, clock: Optional[Callable[[], float]] = None):
        self._clock = clock or time.perf_counter
        self._origin = self._clock()
        self.root = Node("root")
        self._current = self.root
        self.events: List[TraceEvent] = []

    @contextmanager
    def section(self, name: str) -> Iterator[None]:
        """Замеряет блок и автоматически вкладывает его в текущий узел."""
        if not name:
            raise ValueError("Имя измеряемой секции не может быть пустым")

        parent = self._current
        node = parent.child(name)
        start = self._clock()
        self._current = node
        try:
            yield
        finally:
            finish = self._clock()
            duration = max(0.0, finish - start)
            node.calls += 1
            node.total += duration
            self.events.append(TraceEvent(name, start - self._origin, duration))
            # Даже если внутри возникло исключение, возвращаемся к родителю.
            self._current = parent

    def profile(self, function: F) -> F:
        """Декоратор для замера целой функции."""
        @wraps(function)
        def wrapper(*args, **kwargs):
            with self.section(function.__name__):
                return function(*args, **kwargs)
        return wrapper  # type: ignore[return-value]

    def report(self, fmt: ReportFormat) -> str:
        """Паттерн Strategy: формат выбирает вызывающий код, не Profiler."""
        return fmt.render(self)
