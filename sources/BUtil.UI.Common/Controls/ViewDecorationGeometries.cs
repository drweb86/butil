using Avalonia.Media;

namespace BUtil.UI.Controls;

internal static class ViewDecorationGeometries
{
    public static Geometry? Get(ViewDecorationKind kind) =>
        kind switch
        {
            ViewDecorationKind.TasksHome => Parse(TasksHome),
            ViewDecorationKind.Restore => Parse(Restore),
            ViewDecorationKind.LaunchTask => Parse(LaunchTask),
            ViewDecorationKind.TechnicalTool => Parse(TechnicalTool),
            ViewDecorationKind.IncrementalBackup => Parse(IncrementalBackup),
            ViewDecorationKind.Synchronization => Parse(Synchronization),
            ViewDecorationKind.ImportMedia => Parse(ImportMedia),
            ViewDecorationKind.BUtilServer => Parse(BUtilServer),
            ViewDecorationKind.BUtilClient => Parse(BUtilClient),
            _ => null,
        };

    private static Geometry Parse(string path) => Geometry.Parse(path);

    // Tasks list — cat (same path as TasksView)
    private const string TasksHome =
        "m85.09 191.53c-9.639-12.84-11.672-16.17-10.498-32.44 1.538-21.3 1.024-21.3-27.811 0.04-13.26 9.82-15.87 14.6-14.196 26 1.62 11.03 0.188 13.97-6.822 13.97-15.075 0-11.852-33.03 5.248-53.77 7.766-9.42 9.417-22.24 13.333-33.05 5.335-14.729 8.15-26.754 7.036-30.191-6.147-18.961-6.688-27.537-20.197-47.293-21.334-31.199-34.368-29.313-30.509-33.972s25.619 11.278 35.709 26.526c10.09 15.249 17.636 31.58 27.497 36.858 9.862 5.277 96.89 22.977 130.06 11.147 14.09-5.026 32.01-12.343 47.8-10.56 8.63 0.974 14.68-15.681 18.12-12.668 2.73 2.393-4.93 8.999 10.32 20.788 4.21 3.259 5.42 9.542 5.23 14.562-0.63 17.253-10.46 11.468-23.68 11.468-9.89 0-32.42 13.725-31.94 27.835 0.21 6.11 3.91 24.07 11.33 37.55 4.88 8.85 8.35 17.45 11.25 22.69 4.56 8.26 8.22 9.75-1.26 12.08-2.16 0.52-6.84-5.24-10.27-11.64-3.42-6.4-14.58-29.08-22.33-34.81-12.81-9.47-11.03-9.67-13.89-2.22-1.73 4.51-2.14 27.19 0.74 34.81 4.21 11.17 6.1 13.86-1.63 13.86-3.78 0-6.87-4.33-6.87-9.62 0-5.28-3.91-18.25-7.81-29.14-4.31-12.02 1.82-14.64-7.81-19.52s-8.97 0.29-32.08 2.06c-23.11 1.76-30.11-0.98-39.38 12.05-13.218 18.56-12.685 24.64 3.31 37.74 8.6 7.04 8.31 11.45 3.63 11.45-11.584-0.01-15.274-4.12-21.63-12.59z";

    // Restore — 270° CW arc (right→bottom→left→top) with arrowhead and inner ring
    private const string Restore =
        "M 352 100 A 52 52 0 1 1 300 48 L 283 35 M 300 48 L 283 61";

    // Launch task — rocket with fins, porthole and flame
    private const string LaunchTask =
        "M 150 10 C 162 20 178 48 180 85 L 182 152 L 218 188 L 182 168 L 118 168 L 82 188 L 118 152 L 120 85 C 122 48 138 20 150 10 Z " +
        "M 136 168 C 131 185 130 200 150 212 C 170 200 169 185 164 168 Z " +
        "M 132 106 A 18 18 0 0 1 168 106 A 18 18 0 0 1 132 106";

    // Technical tools — padlock (file encryption/security)
    private const string TechnicalTool =
        // shackle — legs meet the top edge of the lock body at y=91
        "M 100 91 L 100 67 C 100 27 200 27 200 67 L 200 91 " +
        // lock body
        "M 46 113 C 46 101 56 91 68 91 L 232 91 C 244 91 254 101 254 113 L 254 195 C 254 207 244 217 232 217 L 68 217 C 56 217 46 207 46 195 Z " +
        // keyhole circle
        "M 130 145 A 20 20 0 0 1 170 145 A 20 20 0 0 1 130 145 " +
        // keyhole slot
        "M 150 165 L 150 187";

    // Incremental backup — vertical timeline with growing horizontal bars
    private const string IncrementalBackup =
        "M 55 30 L 55 195 M 40 55 L 55 30 L 70 55 " +
        "M 55 70 L 110 70 M 55 105 L 160 105 M 55 140 L 210 140 M 55 175 L 260 175 " +
        "M 268 175 A 10 10 0 0 1 288 175 A 10 10 0 0 1 268 175";

    private const string Synchronization =
        // ── left file ──────────────────────────────────────────
        // left file outline (move to top-left, across, fold corner, down, close)
        "M 200 50 L 244 50 L 260 66 L 260 140 L 200 140 Z " +

        // left file folded corner (dog-ear triangle)
        "M 244 50 L 244 66 L 260 66 " +

        // left file content lines (top, middle, short)
        "M 212 88 L 248 88 " +
        "M 212 102 L 248 102 " +
        "M 212 116 L 236 116 " +

        // ── right file ─────────────────────────────────────────
        // right file outline
        "M 420 50 L 464 50 L 480 66 L 480 140 L 420 140 Z " +

        // right file folded corner (dog-ear triangle)
        "M 464 50 L 464 66 L 480 66 " +

        // right file content lines (top, middle, short)
        "M 432 88 L 468 88 " +
        "M 432 102 L 468 102 " +
        "M 432 116 L 456 116 " +

        // ── top arrow: left → right ────────────────────────────
        // cubic bezier curve arcing upward from left file to right file
        "M 268 78 C 310 48 370 48 412 78 " +

        // beak at (412,78) — arms rotated 35° to match curve tangent
        "M 402.8 62.5 L 412 78 L 395.6 75.2 " +

        // ── bottom arrow: right → left ─────────────────────────
        // cubic bezier curve arcing downward from right file to left file
        "M 412 112 C 370 142 310 142 268 112 " +

        // beak at (268,112) — arms rotated -145° to match curve tangent
        "M 277.2 127.5 L 268 112 L 284.4 114.8";

    // Import media — camera body (shifted down) with lens rings and indicator
    private const string ImportMedia =
        "M 88 103 C 88 103 100 80 118 80 L 138 80 L 155 103 L 252 103 C 263 103 270 111 270 122 L 270 190 C 270 201 263 208 252 208 L 48 208 C 37 208 30 201 30 190 L 30 122 C 30 111 37 103 48 103 Z " +
        "M 112 155 A 38 38 0 0 1 188 155 A 38 38 0 0 1 112 155 " +
        "M 124 155 A 26 26 0 0 1 176 155 A 26 26 0 0 1 124 155 " +
        "M 232 125 A 8 8 0 0 1 248 125 A 8 8 0 0 1 232 125";

    // BUtil server — rack shifted right, rounded body with slots and lights
    private const string BUtilServer =
        "M 142 22 C 130 22 120 32 120 44 L 120 190 C 120 202 130 212 142 212 L 262 212 C 274 212 284 202 284 190 L 284 44 C 284 32 274 22 262 22 Z " +
        "M 130 62 L 274 62 M 130 102 L 274 102 M 130 142 L 274 142 M 130 182 L 274 182 " +
        "M 140 44 A 6 6 0 0 1 152 44 A 6 6 0 0 1 140 44 " +
        "M 140 84 A 6 6 0 0 1 152 84 A 6 6 0 0 1 140 84 " +
        "M 140 124 A 6 6 0 0 1 152 124 A 6 6 0 0 1 140 124 " +
        "M 140 164 A 6 6 0 0 1 152 164 A 6 6 0 0 1 140 164 " +
        "M 172 212 L 172 222 L 232 222 L 232 212";

    // BUtil client — desktop monitor with connected client device
    // BUtil client — desktop monitor with connected client device
    private const string BUtilClient =
        // monitor body
        "M 38 30 C 28 30 20 38 20 48 L 20 152 C 20 162 28 170 38 170 L 182 170 C 192 170 200 162 200 152 L 200 48 C 200 38 192 30 182 30 Z " +
        // monitor screen
        "M 36 44 L 182 44 L 182 158 L 36 158 Z " +
        // monitor stand and base
        "M 80 170 L 80 188 L 142 188 L 142 170 M 48 188 L 174 188 " +
        // client device body (shifted +20px right for spacing)
        "M 238 70 C 230 70 225 78 225 88 L 225 162 C 225 172 230 180 238 180 L 300 180 C 308 180 313 172 313 162 L 313 88 C 313 78 308 70 300 70 Z " +
        // client device slots
        "M 235 90 L 303 90 M 235 110 L 303 110 M 235 130 L 303 130 M 235 150 L 303 150 " +
        // connector line between monitor and client device
        "M 200 118 L 225 118";
}
