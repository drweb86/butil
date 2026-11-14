# Play Store listing

One folder per locale the Android app translates, using Play Console locale names. `en-US` is the source. `values-iw` and `values-in` are legacy aliases of Hebrew and Indonesian, so they do not get a second folder.

Changelogs stay in `en-US` only. Screenshots are English only, in `en-US/images`.

| File | Play Console field | Limit |
|---|---|---|
| `title.txt` | App name | 30 characters |
| `short_description.txt` | Short description | 80 characters |
| `full_description.txt` | Full description (HTML) | 4000 characters |
| `full_description_text.txt` | Same text without HTML, for review and other stores | |
| `store_features.txt` | Feature lines, one per line | |
| `language_name.txt` | English name of the language | |
| `changelogs/<versionCode>.txt` | What's new. `versionCode` is the date with dots removed, for example `20261008` | 500 characters |

`en-US/images/phoneScreenshots` holds 7 PNG files, 1269×2256. `en-US/images/tenInchScreenshots` holds 6 JPEG files, 2160×1215. Those sizes match the phone (9:16) and 10-inch (16:9) images used for the other Android app.
