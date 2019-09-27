# DeskRedis
Redis客户端桌面管理工具   
   
Redis Desktop Manager，的确好用，然而开始收费了。   
Redis Client，老掉线断线，算了。   
Redis Studio，讲真不是很好用。   
   
   
所以，模仿Redis Desktop Manager的外观做了一个WPF基本版本的。
履行诺言，在第二个版本（即2.19.0927.1725）对外观做了第一次整改美化。不过，还请大家多多包涵作为一个程序员的审美。   
   
如果你想直接使用完整的软件，可以在Download文件夹下下载DeskRedis.zip包。使用软件但需要安装.NET哦！
   
   
### 更新日志   

[1.19.0816.1802]   
- 修复不包含冒号的键无法显示在列表中的问题。
- 修复当redis中存在WRONGTYPE Operation against a key holding the wrong kind of value错误键时导致程序崩溃的问题。   

[1.19.0823.1551]    
- 修复当redis连接失败时，日志栏中仍然显示连接成功的问题。
- 增加连接信息修改功能。   

[1.19.0826.1611]    
- 修复新增连接时程序可能崩溃的问题。   

[1.19.0830.1613]
- 增加左边redis树形列表滚动条。   

[1.19.0926.1128]
- 增加指定键搜索功能。   

[2.19.0927.1725]
- 美化外观。    
