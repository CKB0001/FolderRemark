# 功能介绍

对Windows 文件夹添加备注，可以使用到鼠标右键菜单，也可以单独使用

# 如何安装
有两种方式，自动安装（安装包）和手动安装
## 自动安装
下载安装包
```tex
install\FolderRemarkInstall.exe
```
运行即可

## 手动安装

下载exe文件

```txt
bin/Release/net6.0/publish/remarkv1.0/FolderRemark.exe
```

如果想要鼠标右键可以使用，需要修改注册表

```txt
计算机\HKEY_CLASSES_ROOT\Directory\shell\
```

这个位置 新建项 FolderRemark 并且修改默认值 “文件夹备注”

然后新建项command 在这个项中添加exe 存放位置，如

```
"D:\\software\\FolderRemark\\FolderRemark.exe" "%V"
```

