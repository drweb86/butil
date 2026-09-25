# Microsoft Store and WinGet metadata

One folder per language. `en-us` is the source. Other languages are added after the English copy is approved.

| File | Store field | WinGet |
|---|---|---|
| `short_description.txt` | Short description, one line, at most 256 characters | ShortDescription |
| `description.txt` | Description | Description, then the feature lines |
| `features.txt` | Feature1–Feature18, one line each, at most 200 characters | Appended to Description |
| `keywords.txt` | SearchTerm1–SearchTerm7, one line each, at most 40 characters, 21 words in total | Tags |
| `release_notes.txt` | Release notes, at most 1500 characters | Not used |

WinGet locale manifests are generated from these folders by `sources/tools/ResxSorter`. The listing text is not stored in `.resx`.
