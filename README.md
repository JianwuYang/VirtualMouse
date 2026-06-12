# VirtualMouse 🖱️ · 虚拟鼠标

[English](#english) | [中文](#中文)

A floating virtual mouse for Windows, designed for touch-based remote desktop / game streaming. When streaming your PC to a tablet or phone, tap the large buttons to send real left / right / middle clicks.

Windows 下的悬浮虚拟鼠标，为触屏串流场景设计。把电脑画面串流到平板或手机时，点击虚拟鼠标的大按钮即可发出真实的左键 / 右键 / 中键点击。

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![WPF](https://img.shields.io/badge/UI-WPF-0078D4)
![Windows](https://img.shields.io/badge/Windows-10%2B-0078D6?logo=windows)

---

## English

### How It Works

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

1. **Position** — Drag the window so the red dot points where you want to click.
2. **Tap** — Press the virtual left / middle / right button.
3. **Click** — A real mouse click fires at the red dot's screen position.

The window is always-on-top and semi-transparent, so you can see content behind it.

### Features

- Always-on-top floating window with per-pixel alpha transparency
- Left / middle / right mouse button emulation
- Click-through during click operations — clicks reach the target app, not the virtual mouse itself
- Realistic mouse-shaped UI with dedicated drag area
- Cursor position auto-restore after clicks
- Esc key, close button, or right-click menu to exit

### Requirements

- Windows 10 or later
- [.NET 10.0 Runtime](https://dotnet.microsoft.com/download) (or SDK to build)

### Build & Run

```bash
git clone https://github.com/YOUR_USERNAME/VirtualMouse.git
cd VirtualMouse
dotnet run
```

Standalone publish:

```bash
dotnet publish -c Release -o ./publish
# Run: ./publish/VirtualMouse.exe
```

### Tech Stack

- **.NET 10 + WPF** — per-pixel alpha compositing, smooth anti-aliased UI
- **P/Invoke** — `mouse_event` for input injection, `WS_EX_TRANSPARENT` for click-through
- Zero third-party dependencies

---

## 中文

### 工作原理

1. **定位** — 拖动窗口，让红色圆点对准要点击的位置。
2. **点击** — 按下虚拟鼠标的左键 / 中键 / 右键按钮。
3. **执行** — 真实的鼠标点击在红色圆点所在的屏幕位置触发。

窗口始终置顶并且半透明，不会完全遮挡后方内容。

### 功能特性

- 悬浮置顶窗口，逐像素 alpha 半透明，圆角无锯齿
- 支持左键、中键、右键模拟
- 点击瞬间窗口自动穿透（`WS_EX_TRANSPARENT`），确保点击落到目标应用上
- 鼠标造型 UI，包含左右按键、滚轮中键和拖拽区
- 点击后光标自动恢复原位
- Esc 键 / 关闭按钮 / 右键菜单均可退出

### 运行环境

- Windows 10 或更高版本
- [.NET 10.0 运行时](https://dotnet.microsoft.com/download)（或 SDK 用于源码编译）

### 构建 & 运行

```bash
git clone https://github.com/YOUR_USERNAME/VirtualMouse.git
cd VirtualMouse
dotnet run
```

独立发布：

```bash
dotnet publish -c Release -o ./publish
# 运行: ./publish/VirtualMouse.exe
```

### 技术栈

- **.NET 10 + WPF** — 逐像素 alpha 合成，平滑抗锯齿渲染
- **P/Invoke** — `mouse_event` 注入输入，`WS_EX_TRANSPARENT` 实现点击穿透
- 无第三方依赖

---

## License · 许可

MIT

---

*🤖 This project was built with [Claude Code](https://claude.ai/code). · 本项目由 AI 编程助手 Claude Code 辅助实现。*
