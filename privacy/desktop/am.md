[Languages](README.md)

# ግላዊነት ፖሊሲ

መጨረሻ የተዘመነው፦ 25 መስከረም 2026

**BUtil** by Siarhei Kuchuk

የመተግበሪያ ስም፦ BUtil
የገንቢ ስም፦ Siarhei Kuchuk

BUtil በዚህ ኮምፒውተር ላይ ፋይሎችን ይቀመጣል፣ ያመሳስላል እና ይመልሳል። ሚዲያ ማስገባት፣ አቃፊ ማጋራት ወይም ፋይሎችን ወደ እርስዎ ወደሚያዘጋጁት አገልጋይ መስቀልም ይችላል። የገንቢ መለያ አይፈጥርም። ገንቢው ፋይሎችዎን፣ የይለፍ ቃሎችዎን ወይም የአጠቃቀም ውሂብዎን የሚቀበል አገልጋይ አያስኬድም።

## ገንቢው የማይሰበስበው ውሂብ

መተግበሪያው ማስታወቂያ፣ ትንታኔ፣ የብልሽት ሪፖርት ወይም የመከታተያ SDK የለውም። ገንቢው ግላዊ ውሂብ አይሰበስብም፣ አይሸጥም እና አያጋራም።

## በኮምፒውተርዎ ላይ የሚቀመጥ ውሂብ

### ተግባራት እና ቅንብሮች

የተግባር ትርጓሜዎች በዚህ ኮምፒውተር ላይ ብቻ ይቀመጣሉ። ተግባር የአቃፊ መንገዶችን፣ መርሐ ግብርን፣ የማከማቻ ቅንብሮችን እና የሚተይቧቸውን የይለፍ ቃሎች ወይም ቶከኖች ሊይዝ ይችላል። የይለፍ ቃሎች እና የማከማቻ ምስጢሮች ከመቀመጣቸው በፊት በዚህ ኮምፒውተር ላይ ይመሰጠራሉ፣ እና በዚህ ኮምፒውተር ላይ ብቻ ሊነበቡ ይችላሉ። እነዚህ እሴቶች ለገንቢው አይላኩም።

- የWindows ተግባራት፦ `%AppData%\BUtil Backup Tasks`
- የLinux ተግባራት፦ `~/.config/BUtil Backup Tasks`
- የWindows ቅንብሮች (ገጽታን እና ለፈቃድ ወይም ለግላዊነት መጨረሻ የተመረጠውን ቋንቋ ጨምሮ)፦ `%AppData%\BUtil\Settings\v1`
- የLinux ቅንብሮች፦ `~/.config/BUtil/Settings/v1`
- የWindows ተግባር ሁኔታ፦ `%AppData%\BUtil\States`
- የLinux ተግባር ሁኔታ፦ `~/.config/BUtil/States`
- የWindows የሚዲያ ማስገቢያ ሁኔታ፦ `%AppData%\BUtil Backup Tasks - States`
- የLinux የሚዲያ ማስገቢያ ሁኔታ፦ `~/.config/BUtil Backup Tasks - States`

### የሚመርጧቸው ፋይሎች

መጠባበቂያ፣ ማመሳሰል፣ መመለስ እና ማስገባት የሚመርጧቸውን አቃፊዎች ያነባሉ እና ይጽፋሉ። እነዚህ ፋይሎች በዚህ ኮምፒውተር ወይም በሚያዘጋጁት ማከማቻ ላይ ይቆያሉ። መተግበሪያው ለገንቢው አይልካቸውም።

### መዝገቦች

የምርመራ መዝገቦች በዚህ ኮምፒውተር ላይ ብቻ ይጻፋሉ፦

- Windows፦ `%LocalAppData%\BUtil\logs\v4`
- Linux፦ `~/.local/share/BUtil/logs/v4`

እነዚህ ፋይሎች ወደ የትም አይላኩም።

ውሂብዎን ለማከማቸት የገንቢ አገልጋይ አይጠቀምም።

## የአውታረ መረብ አጠቃቀም

### የሚያዘጋጇቸው መድረሻዎች

ተግባር ሲሠራ መተግበሪያው ወደ እርስዎ ወደሚወስኑት ቦታ ብቻ ይገናኛል። ይህ የአካባቢ አቃፊ ወይም የሚያስገቡት አገልጋይ ሊሆን ይችላል፦ FTP፣ FTPS፣ SFTP፣ WebDAV፣ SMB፣ NFS፣ ከS3 ጋር የሚጣጣም ማከማቻ ወይም Azure Blob Storage። የፋይል ስሞች፣ ይዘት እና ያስገቡት ምስክርነቶች ተግባሩ እንዲሠራ ወደዚያ አገልጋይ ይላካሉ። እያንዳንዱ አገልግሎት የራሱ የግላዊነት ፖሊሲ አለው። ገንቢው ይህን ትራፊክ አይቀበልም።

BUtil Server እርስዎ የሚያዘጋጁት BUtil ደንበኛ ፋይሎችን እንዲልክ በዚህ ኮምፒውተር ላይ ማዳመጥ ይችላል። ይህ ትራፊክ በሚያዘጋጇቸው ኮምፒውተሮች መካከል ይቆያል።

### የዝማኔ ፍተሻ

ከStore ውጭ ያሉ ግንባታዎች የቅርብ GitHub እትም ሊጠይቁ ይችላሉ፦

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) ተራ HTTPS ጥያቄ ይቀበላል (IP አድራሻ፣ user-agent፣ ሰዓት)። ገንቢው ይህን ትራፊክ አይቀበልም።

ከMicrosoft Store የሚጫኑት ይህን ፍተሻ አይጠቀሙም፤ ዝማኔዎችን Store ያቀርባል።

### የሚከፍቷቸው አገናኞች

መተግበሪያው እነዚህን ገጾች በስርዓት አሳሽ ውስጥ ሊከፍት ይችላል። እነዚህ ጣቢያዎች የራሳቸው የግላዊነት ፖሊሲ አላቸው፦

- የፕሮጀክት መነሻ ገጽ፦ [github.com/drweb86/butil](https://github.com/drweb86/butil)
- የቅርብ እትም፦ [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- የፋይል ንድፍ እገዛ፦ [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- የቀን ቅርጸት እገዛ፦ [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- የአዶ ምንጮች፦ [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

ፈቃዱ እና ይህ የግላዊነት ፖሊሲ በመተግበሪያው ውስጥ ይታያሉ። እንደ ድር ገጾች አይከፈቱም።

## መርሐ ግብር

በWindows ላይ ተግባርን በመግባት ጊዜ ወይም በሳምንታዊ መርሐ ግብር ማሄድ ይችላሉ። መተግበሪያው ይህን በWindows Task Scheduler ውስጥ በ`BUtil` በሚጀምር ስም ይመዘግባል። ይህ ይህን መተግበሪያ በኮምፒውተርዎ ላይ ብቻ ያስጀምራል።

## ልጆች

መተግበሪያው የመጠባበቂያ እና የፋይል ማመሳሰል መሣሪያ ነው። ከ13 ዓመት በታች ለሆኑ ልጆች አልተዘጋጀም።

## ሦስተኛ ወገኖች

GitHub የዝማኔ ፍተሻ ጥያቄውን እና የሚከፍቷቸውን ገጾች ከላይ እንደተገለጸው ያስኬዳል። Microsoft Store የStore ጭነቶችን እና ዝማኔዎችን ያስኬዳል። እርስዎ የሚያዘጋጇቸው የማከማቻ አቅራቢዎች ተግባሩ የሚልካቸውን ፋይሎች እና ምስክርነቶች ያስኬዳሉ። ገንቢው ይህን ትራፊክ አይቀበልም።

## ለውጦች

የዚህ ፖሊሲ ዝማኔዎች በፕሮጀክቱ ማከማቻ ውስጥ በዚህ ፋይል ይታተማሉ።

## ግንኙነት

የመተግበሪያ ስም፦ BUtil
የገንቢ ስም፦ Siarhei Kuchuk

ጥያቄዎች፦ [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
