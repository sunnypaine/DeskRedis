# DeskRedis
Redis客户端桌面管理工具   
   
Redis Desktop Manager，的确好用，然而开始收费了。   
Redis Client，老掉线断线，算了。   
Redis Studio，讲真不是很好用。   
   
   
所以，模仿Redis Desktop Manager的外观做了一个WPF基本版本的。
下一个版本对外观进行美颜化妆。   
   
如果你想直接使用完整的软件，可以在Download文件夹下下载DeskRedis.zip包。使用软件但需要安装.NET哦！
   
   
### 更新日志
[1.0.1908.1802]   
- 修复不包含冒号的键无法显示在列表中的问题。
- 修复当redis中存在WRONGTYPE Operation against a key holding the wrong kind of value错误键时导致程序崩溃的问题。