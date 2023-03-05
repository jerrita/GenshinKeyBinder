# GenshinMidiBinder

BOME MIDI Player 替代品，但是很简陋

- 这是一个 Midi 键盘转键盘输入的小程序，可以用来弹琴（x
- 目前只映射了 C4 - A4 到 `ASDJKL`，可以用来打曲子
- A3 映射成了空格，可以用来调延迟
- 如果你有其他需求，可以在代码里添加映射，如果可以的话希望您能提一个 PR
- 不知道这种实现会不会导致封号，可能换用 DirectX 的 `SendInput` 会更稳妥一些

## 实现方式

- https://github.com/naudio/NAudio
- https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes