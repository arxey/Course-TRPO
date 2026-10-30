"""Демонстрация: 60 игровых кадров и 361 событие временной шкалы.

Запуск: python main.py
"""

import argparse
import time

from profiler import Profiler
from reports import JSONReport, TableReport, TreeReport


def run_demo(frames: int = 60, trace: str = "results/trace.json",
             slow_frame: int = 30) -> Profiler:
    profiler = Profiler()
    current_frame = -1

    @profiler.profile
    def physics():
        time.sleep(0.002)

    @profiler.profile
    def shadows():
        # На одном кадре намеренно появляется пик (дополнительные 20 мс).
        time.sleep(0.003 + (0.020 if current_frame == slow_frame else 0.0))

    @profiler.profile
    def sprites():
        time.sleep(0.0015)

    @profiler.profile
    def update():
        physics()
        sum(range(30_000))  # собственное время update

    @profiler.profile
    def draw():
        shadows()
        sprites()

    with profiler.section("загрузка уровня"):
        time.sleep(0.200)

    for current_frame in range(frames):
        with profiler.section("кадр"):
            update()
            draw()

    print(profiler.report(TreeReport()))
    print()
    print(profiler.report(TableReport()))
    print()
    print(profiler.report(JSONReport(trace)))
    print(f"Кадров: {frames}; пример пика: кадр №{slow_frame + 1} "
          f"(нумерация с 1), если он входит в диапазон")
    return profiler


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Практическая №2: вложенный профилировщик")
    parser.add_argument("--frames", type=int, default=60, help="Количество кадров (по умолчанию 60)")
    parser.add_argument("--trace", default="results/trace.json", help="Путь к Chrome Trace JSON")
    parser.add_argument("--slow-frame", type=int, default=30,
                        help="Индекс медленного кадра, начиная с нуля")
    args = parser.parse_args()
    if args.frames < 1:
        parser.error("--frames должен быть положительным")
    run_demo(frames=args.frames, trace=args.trace, slow_frame=args.slow_frame)
