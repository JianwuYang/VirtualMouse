# VirtualMouse 🖱️

A floating virtual mouse for Windows, designed for touch-based remote desktop / game streaming scenarios. When streaming your PC to a tablet or phone, touching the screen to click can be imprecise — VirtualMouse gives you large, easy-to-tap buttons that send real left/right/middle clicks at a target position on screen.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![WPF](https://img.shields.io/badge/UI-WPF-0078D4)
![Windows](https://img.shields.io/badge/Windows-10%2B-0078D6?logo=windows)

## How It Works

```
     ●  ← pointer (click lands here)
     │
╭──────────────╮
│ ┌────┬────┐ │
│ │LEFT│RGHT│ │  ← tap to click at ●
│ └────┴────┘ │
│    ═══     │  ← middle button
│   ⋮⋮⋮     │  ← drag to reposition
╰──────────────╯
```

1. **Position** — Drag the window so the red dot points where you want to click
2. **Tap** — Press the virtual left / middle / right button
3. **Click** — The real click fires at the red dot's screen position

The window stays always-on-top and is semi-transparent so you can see content behind it.

## Features

- Always-on-top floating window with per-pixel alpha transparency
- Left, middle, and right mouse button emulation
- Click-through during click operations — clicks reach the target application, not the virtual mouse itself
- Realistic mouse-shaped UI with drag handle
- Cursor position auto-restore after clicks
- Escape key or close button to exit

## System Requirements

- Windows 10 or later
- [.NET 10.0 Runtime](https://dotnet.microsoft.com/download) (or SDK to build from source)

## Build & Run

```bash
git clone https://github.com/YOUR_USERNAME/VirtualMouse.git
cd VirtualMouse
dotnet run
```

To produce a standalone executable:

```bash
dotnet publish -c Release -o ./publish
# Run: ./publish/VirtualMouse.exe
```

## Tech Stack

- **.NET 10 + WPF** — Native Windows UI with per-pixel alpha compositing
- **P/Invoke** — `mouse_event` for input injection, `WS_EX_TRANSPARENT` for click-through
- No third-party dependencies

## License

MIT

---

*This project was built with [Claude Code](https://claude.ai/code) — an AI coding assistant.*
