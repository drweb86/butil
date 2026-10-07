# Play Store listing

One folder per Play Console locale. `en-US` is the source. Other locales are added later, the same way as `fastlane/metadata/microsoft`.

The application in `sources/android` is still a stub. This listing is the draft text for the product.

| File | Play Console field | Limit |
|---|---|---|
| `title.txt` | App name | 30 characters |
| `short_description.txt` | Short description | 80 characters |
| `full_description.txt` | Full description (HTML) | 4000 characters |
| `full_description_text.txt` | Same text without HTML, for review and other stores | |
| `store_features.txt` | Feature lines, one per line | |
| `language_name.txt` | English name of the language | |
| `changelogs/<versionCode>.txt` | What's new. `versionCode` is the date with dots removed, for example `20261008` | 500 characters |

Screenshots go in `images/phoneScreenshots` and `images/tenInchScreenshots` when the app has a real UI.
