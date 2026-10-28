## Decision records

A decision record explains one decision whose reasons the code does not show:
what was decided, why, and which alternatives were rejected, and by whom.
Records live in `docs/decisions/`, one file per decision, named
`NNNN-a-few-words.md`. `ADR-NNNN` anywhere in the repository cites the record
with that number. `docs/DECISIONS.md` holds the conventions, the template, and
the index: one row per record, grouped by area.

- **Read before you change.** Before changing code that cites a record, read the
  record and every record that amends it, directly or through another
  amendment; the _amended by_ notes in the index list them. Then search the
  repository for the record's number: every citation marks code or a test that
  relies on the decision. Before changing behaviour in an area, read that
  area's rows in the index. Do not propose an option a record rejected unless
  something has changed since, and then say what. If a record says the user
  made the decision or rejected the option, only the user can reverse it.
- **Write one when a reader would get it wrong.** A change needs a record when a
  competent reader would take it for a mistake; when it departs from a library
  or framework default, from the project's documentation, or from an earlier
  record; when it rejects an option that is likely to come up again; or when the
  user makes a decision in conversation that the code will not show. A bug fix,
  a refactor, or a feature built the obvious way does not. Write the record in
  the same change as the code: copy the template, take the next free number,
  title it with the decision as a sentence ("Sessions expire after 30 days
  without use", not "Sessions"), and add a row with that title to the index.
- **Cite, don't copy.** A decision's reasons live in its record and nowhere
  else. A comment at the code that implements the decision says what the code
  does and cites the record, as in `# Expire idle sessions (ADR-NNNN).` A test
  that enforces the decision cites the record too. Neither repeats the reasons:
  two copies drift apart, and no test can tell which one is current.
- **Amend, don't rewrite.** Never change what an accepted record decided;
  correcting a stale path or name in it is fine. A change of mind is a new
  record whose status line ends with `_amends ADR-NNNN_`, and the amended
  record's row in the index gets `_amended by ADR-NNNN_`. When finding out what
  a decision says today means reading more than four records, write one record
  that states it in full and amends all of them.
- **Keep the check green.** `tests/test_decision_records.py` fails when a
  citation names no record, when the index and `docs/decisions/` disagree, or
  when `AGENTS.md` or `CLAUDE.md` outgrows its byte budget. Fix the citation,
  the index, or the oversized file, not the check. Raise the budget only when
  the user asks.
- **Keep `AGENTS.md` and `CLAUDE.md` short.** They say how to work in this
  repository and where to look. A decision belongs in a record: these files may
  name the record, but do not restate it. When the check reports either file
  over its budget, move decisions out of it into records.