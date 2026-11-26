# 血滴文字 Blood Drip Text

给 TextMeshPro 用的「血往下淌」文字效果。基于 TMP 的 SDF 距离场，在字形底部生成随机长度、缓慢脉动的血滴痕迹。

## 依赖

- Unity 2021.3+（任意渲染管线）
- TextMeshPro（Package Manager 里安装，默认已随项目导入）

## 安装

把整个 `BloodDripText` 文件夹拷进项目的 `Assets/`，或用菜单 `Tools/血滴字体/③ 导出 unitypackage` 导出后给别人导入。

## 三步使用

1. **选中字体文件**（`.ttf` / `.otf`），执行菜单 `Tools/血滴字体/① 生成血滴字体`
   → 自动生成 `xxx Blood SDF.asset`（字体资产）+ `xxx Blood SDF Bleed.mat`（血滴材质）
2. 在场景里选中 `TextMeshPro - Text` 组件，把 **Font Asset** 换成刚生成的 `xxx Blood SDF`
3. 完成。文字自动变为深红字面 + 底部血滴，且随时间缓慢流动

已有 SDF 字体想换效果：选中该 Font Asset → `Tools/血滴字体/② 给已有字体换血滴材质`

## 参数（材质 Inspector 里调）

| 参数 | 说明 | 默认 |
|---|---|---|
| Face Color | 字面颜色 | 深红 (0.55, 0.02, 0.02) |
| Drip Color | 血滴颜色 | 亮红 (0.8, 0.02, 0.02) |
| Drip Length | 血滴长度（相对字形大小） | 1.2 |
| Drip Density | 有多少列会流（0=不流，1=全流） | 0.55 |
| Drip Width | 血滴柱宽度 | 2.5 |
| Drip Speed | 流动/脉动速度，0 = 静止 | 0.6 |
| Drip Opacity | 血滴不透明度 | 0.95 |

## 重要限制

**血滴只会出现在字形的 padding 区域内。** 生成器用了 `atlasPadding = 12`（比默认 5 大），这决定了血滴最长能拖多远。

如果想要更长的血流：
- 改 `BloodDripFontSetup.cs` 里 `CreateFontAsset(font, 90, 12, ...)` 的第三个参数，比如 `20`
- padding 越大，字体图集占用越大，血滴能拖得更长
- 或者配合材质里的 `Drip Length` 一起调

**字体本身要包含你显示的文字。** 比如 Cooper Black 只有英文，中文会显示成 □。

## 中文怎么做

1. 把中文字体（`.ttf`，例如 `simhei.ttf`）放进 `Assets/Fonts/`
2. 选中它 → `Tools/血滴字体/① 生成血滴字体`，得到 `xxx Blood SDF`
3. 选中你的**主字体资产**（英文那个）→ Inspector 最下方 **Fallback Font Assets** → 把中文的 `xxx Blood SDF` 加进去
4. 一个 TMP 文本里就能中英混排，两边都带血滴

注意：用的是 Windows 自带字体（如 SimHei）时，**不要把它随项目公开发布**，有版权问题；商用建议换成思源黑体 / Noto Sans SC 等开源字体。

## 兼容性说明

- Shader 自带全部 uniform 声明，**不依赖 TMP 的 cginc 路径**，TMP 目录被改名/移动也能编译
- 支持 UI（Canvas）与 3D（TextMeshPro）两种用法，支持 UI 裁剪与遮罩
- 没有走 TMP 的自定义材质面板（CustomEditor），参数在材质 Inspector 里直接调
