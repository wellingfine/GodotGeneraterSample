# Godot 导出属性汇总示例

这是一个 Godot C# 源代码生成器示例：在编译时，为标注了 `[GenerateExportSummary]` 的节点类生成 `DescribeExports()` 方法。方法在运行时读取该类的 `[Export]` 成员，并返回每行 `名称=值` 的文本。示例节点在 `_Ready()` 中将结果打印到 Godot 输出窗口。

## 工程组成

- `project.godot`：Godot 项目配置，主场景为 `Main.tscn`。
- `Main.tscn`：挂载 `Scripts/ExportDemo.cs`，将导出属性 `Health`、`DisplayName` 分别设为 `42` 和 `Scene value`。
- `Scripts/ExportDemo.cs`：示例节点，声明导出属性并调用生成的方法。
- `ExportSummaryGenerator/`：独立的 Roslyn 增量源代码生成器项目；主工程通过 Analyzer 引用它，生成器不会作为游戏运行时程序集加载。标记特性 `[GenerateExportSummary]` 也由生成器在编译时自动注入（`GenerateExportSummaryAttribute.g.cs`），无需手写。

## 构建和运行

需要 .NET 10 SDK、可用的 Godot .NET 4.7.2 环境，以及能够还原 NuGet 依赖的网络或本地缓存。在项目根目录执行：

```sh
dotnet build ExportDemo.csproj
```

然后用 Godot .NET 编辑器打开本目录并运行主场景。Godot 的输出窗口应显示以下两行（顺序以生成结果为准）：

```text
Health=42
DisplayName=Scene value
```

脚本中 `Health = 75`、`DisplayName = "Hero"` 是默认值；运行 `Main.tscn` 时，场景保存的值会覆盖它们，因此输出的是 `42` 和 `Scene value`。

## 生成规则

- 目标类必须带 `[GenerateExportSummary]`，且是顶层、非泛型的 `partial class`；否则生成器报编译错误 `EGD001`。
- 只收集目标类自身声明的非静态 `[Export]` 字段，以及具有 getter 的非索引 `[Export]` 属性；不遍历继承的成员。输出不承诺固定顺序。
- `DescribeExports()` 在调用时读取当前成员值，而不是在编译时保存默认值；每个条目末尾包含换行。它适合调试展示，不是结构化序列化格式。
- 编译生成的文件可以在主工程的中间输出目录（例如 `.godot/mono/temp/obj/Generated/`）查看；这些文件由编译器管理，不应手动修改。

工程目前没有自动化测试；可以通过构建以及运行 `Main.tscn` 来验证生成器与场景覆盖值。
