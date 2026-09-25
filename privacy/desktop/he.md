[Languages](README.md)

# מדיניות פרטיות

עודכן לאחרונה: 25 בספטמבר 2026

**BUtil** by Siarhei Kuchuk

שם היישום: BUtil
שם המפתח: Siarhei Kuchuk

BUtil מגבה, מסנכרן ומשחזר קבצים במחשב זה. הוא יכול גם לייבא מדיה, לשתף תיקייה או להעלות קבצים לשרת שתגדירו. הוא אינו יוצר חשבון מפתח. המפתח אינו מפעיל שרת שמקבל את הקבצים, הסיסמאות או נתוני השימוש שלכם.

## נתונים שהמפתח אינו אוסף

היישום אינו כולל פרסומות, ניתוח נתונים, דיווח על קריסות או ערכות SDK למעקב. המפתח אינו אוסף, מוכר או משתף נתונים אישיים.

## נתונים שנשמרים במחשב שלכם

### משימות והגדרות

הגדרות המשימות נשמרות רק במחשב זה. משימה יכולה לכלול נתיבי תיקיות, לוח זמנים, הגדרות אחסון וסיסמאות או אסימונים שתקלידו. סיסמאות וסודות אחסון מוצפנים במחשב זה לפני השמירה, וניתן לקרוא אותם רק במחשב זה. ערכים אלה אינם נשלחים למפתח.

- משימות Windows: `%AppData%\BUtil Backup Tasks`
- משימות Linux: `~/.config/BUtil Backup Tasks`
- הגדרות Windows (כולל ערכת הנושא והשפה שנבחרה לאחרונה עבור הרישיון או הפרטיות): `%AppData%\BUtil\Settings\v1`
- הגדרות Linux: `~/.config/BUtil/Settings/v1`
- מצב משימות Windows: `%AppData%\BUtil\States`
- מצב משימות Linux: `~/.config/BUtil/States`
- מצב ייבוא מדיה ב-Windows: `%AppData%\BUtil Backup Tasks - States`
- מצב ייבוא מדיה ב-Linux: `~/.config/BUtil Backup Tasks - States`

### קבצים שתבחרו

גיבוי, סנכרון, שחזור וייבוא קוראים וכותבים לתיקיות שתבחרו. הקבצים האלה נשארים במחשב זה או ביעד האחסון שתגדירו. היישום אינו מעלה אותם למפתח.

### יומנים

יומני אבחון נכתבים רק במחשב זה:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

הקבצים האלה אינם נשלחים לשום מקום.

לא נעשה שימוש בשרת של המפתח כדי לאחסן את הנתונים שלכם.

## שימוש ברשת

### יעדים שתגדירו

כאשר משימה פועלת, היישום מתחבר רק למקום שהגדרתם. זה יכול להיות תיקייה מקומית או שרת שתזינו: FTP,‏ FTPS,‏ SFTP,‏ WebDAV,‏ SMB,‏ NFS, אחסון תואם S3 או Azure Blob Storage. שמות קבצים, תוכן הקבצים ופרטי הגישה שהזנתם נשלחים לשרת הזה כדי שהמשימה תוכל לרוץ. לכל אחד מהשירותים האלה יש מדיניות פרטיות משלו. המפתח אינו מקבל את התעבורה הזו.

BUtil Server יכול להאזין במחשב זה כדי שלקוח BUtil שתגדירו יוכל לשלוח קבצים. התעבורה הזו נשארת בין המחשבים שהגדרתם.

### בדיקת עדכונים

בניות שאינן מהחנות עשויות לבקש את המהדורה האחרונה ב-GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub‏ (Microsoft) מקבל בקשת HTTPS רגילה (כתובת IP,‏ user-agent, שעה). המפתח אינו מקבל את התעבורה הזו.

התקנות מ-Microsoft Store אינן משתמשות בבדיקה זו; החנות מספקת את העדכונים.

### קישורים שתפתחו

היישום יכול לפתוח את הדפים האלה בדפדפן המערכת. לאתרים האלה יש מדיניות פרטיות משלהם:

- דף הבית של הפרויקט: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- המהדורה האחרונה: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- עזרה לתבניות קבצים: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- עזרה לתבנית תאריך: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- קרדיט לאייקונים: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

הרישיון ומדיניות הפרטיות הזו מוצגים בתוך היישום. הם אינם נפתחים כדפי אינטרנט.

## תזמון

ב-Windows אפשר להפעיל משימה בעת הכניסה או לפי לוח זמנים שבועי. היישום רושם זאת במתזמן המשימות של Windows בשם שמתחיל ב-`BUtil`. זה רק מפעיל את היישום הזה במחשב שלכם.

## ילדים

היישום הוא כלי לגיבוי ולסנכרון קבצים. הוא אינו מיועד לילדים מתחת לגיל 13.

## צדדים שלישיים

GitHub מעבד את בקשת בדיקת העדכונים ואת הדפים שתפתחו, כפי שתואר למעלה. Microsoft Store מעבד התקנות ועדכונים מהחנות. ספקי האחסון שתגדירו מעבדים את הקבצים ואת פרטי הגישה שהמשימה שולחת אליהם. המפתח אינו מקבל את התעבורה הזו.

## שינויים

עדכונים למדיניות זו יפורסמו בקובץ זה במאגר הפרויקט.

## יצירת קשר

שם היישום: BUtil
שם המפתח: Siarhei Kuchuk

שאלות: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
