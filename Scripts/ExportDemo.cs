// 引入 Godot 的 C# API，例如 Node、[Export] 和 GD.Print。
using Godot;

namespace ExportDemo;

// [GenerateExportSummary] 告诉源代码生成器：请为这个类生成 DescribeExports() 方法。
//
// partial 表示“这个类的代码可以分散在多个文件里”。
// 我们在这里写一部分，生成器在编译时再写另一部分（DescribeExports），
// 编译器最终会把它们合并成一个完整的类。没有 partial，生成器就无法给类添加方法。
// Godot 自带的生成器也需要 partial，因此 Godot C# 脚本类通常都要这样声明。
//
// 继承 Node 后，这个类就是 Godot 的一个节点脚本，可以挂到场景里的节点上（见 Main.tscn）。
[GenerateExportSummary]
public partial class ExportDemo : Node
{
	// [Export] 会把属性显示在 Godot 编辑器的“检查器”面板中，让你不改代码就能调整数值。
	// PropertyHint.Range 加上 "0,100,1" 表示：取值范围是 0 到 100，步长为 1，编辑器会显示滑块。
	[Export(PropertyHint.Range, "0,100,1")]
	public int Health { get; set; } = 75;

	// 不带参数的 [Export] 会使用 Godot 根据类型推断的默认编辑方式；string 类型会显示为文本框。
	// Main.tscn 中保存了 DisplayName = "Scene value"，因此运行时会覆盖这里的 "Hero"。
	[Export]
	public string DisplayName { get; set; } = "Hero";

	// _Ready() 是 Godot 的生命周期方法：节点及其子节点都进入场景树后，Godot 会自动调用它一次。
	// 此时场景中保存的导出值已经设置到属性上，所以这里读取到的是 42 和 "Scene value"。
	public override void _Ready()
	{
		// DescribeExports() 不在这个文件里，它是生成器在编译时自动生成的方法。
		// 如果想查看生成后的代码，可以在构建后打开
		// .godot/mono/temp/obj/Generated/ 目录下名字包含 ExportSummary 的 .g.cs 文件。
		//
		// GD.Print 会把文本输出到 Godot 编辑器底部的“输出”面板。
		GD.Print(DescribeExports());
	}
}
