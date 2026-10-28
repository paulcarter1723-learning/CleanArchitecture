"""Every ADR-NNNN in the tree names a record, and the index lists every record."""

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RECORDS = ROOT / "docs" / "decisions"
INDEX = ROOT / "docs" / "DECISIONS.md"

# Where citations can appear, and in which files. Adjust both to your tree.
SCANNED = ["src", "tests", "docs", "AGENTS.md", "CLAUDE.md"]
SUFFIXES = {".py", ".ts", ".tsx", ".md", ".cs"}
SKIPPED = {"node_modules", "dist", "__pycache__"}

CITATION = re.compile(r"ADR-\d{4}")
HEADING = re.compile(r"^# (ADR-\d{4})", re.MULTILINE)
ROW = re.compile(r"\[(ADR-\d{4})\]\(decisions/([^)]+)\)")
IGNORE = "adr-check: ignore"  # on a line that mentions a number it doesn't cite

# AGENTS.md and CLAUDE.md stay short. Set the budget a little above their size
# today, and raise it on purpose, never to get a build green.
INSTRUCTIONS = ["AGENTS.md", "CLAUDE.md"]
BUDGET = 40_000  # bytes


def records() -> dict[str, str]:
    """Map each record's number to the file that defines it."""
    found = {}
    for path in RECORDS.glob("*.md"):
        for number in HEADING.findall(path.read_text(encoding="utf-8")):
            found[number] = path.name
    return found


def citations() -> list[str]:
    """Return 'file:line ADR-NNNN' for every citation in the scanned tree."""
    paths: list[Path] = []
    for entry in SCANNED:
        path = ROOT / entry
        paths.extend([path] if path.is_file() else path.rglob("*"))
    sites = []
    for path in paths:
        relative = path.relative_to(ROOT)
        if not path.is_file() or path.suffix not in SUFFIXES:
            continue
        if SKIPPED & set(relative.parts):
            continue
        lines = path.read_text(encoding="utf-8", errors="ignore").splitlines()
        for number, line in enumerate(lines, start=1):
            if IGNORE not in line:
                sites += [
                    f"{relative}:{number} {adr}" for adr in CITATION.findall(line)
                ]
    return sites


def test_every_citation_names_a_record() -> None:
    known = records()
    dangling = [site for site in citations() if site.split()[-1] not in known]
    assert not dangling, "cites a record that does not exist:\n" + "\n".join(dangling)


def test_the_index_and_the_records_agree() -> None:
    known = records()
    indexed = dict(ROW.findall(INDEX.read_text(encoding="utf-8")))
    assert indexed == known, "docs/DECISIONS.md and docs/decisions/ disagree"


def test_the_instructions_stay_within_budget() -> None:
    for name in INSTRUCTIONS:
        path = ROOT / name
        if path.is_file():
            size = path.stat().st_size
            assert size <= BUDGET, f"{name} is {size} bytes; move decisions to records"