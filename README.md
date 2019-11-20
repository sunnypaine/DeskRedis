# DeskRedis
Redis客户端桌面管理工具   
   
Redis Desktop Manager，的确好用，然而开始收费了。   
Redis Client，老掉线断线，算了。   
Redis Studio，讲真不是很好用。   
   
   
所以，粗略模仿Redis Desktop Manager的外观做了一个WPF基本版本的。
履行诺言，在第二个版本（即2.19.0927.1725）对外观做了第一次整改美化。
**不过，还请大家多多包涵作为一个程序员的审美。**   
   
我的IDE是Visual Studio 2019，不过这不影响各位大佬们使用其他版本（例如2013、2015、2017等，2010可能有点悬）。   
如果你想直接使用完整的软件，可以在Download文件夹下下载DeskRedis.zip包。使用软件需要安装.NET（4.7.2）哦！<br><br>   
   
   
<font face="黑体" color=red size=3>**[\*未曾料及本开源项目能得到各位大佬的关注，项目还很不成熟，所以还请各位大佬提出宝贵意见（不论是建设性的、还是批评的），在下感激不尽。由于时间有限，虽然不能第一时间修改问题，但是一定会跟进并解决，而不是空头支票。]**</font>
   
   
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

[2.19.1119.1922]
- 修复当修改已有连接后，双击根节点报空指针异常的问题。
- 修复当根节点未打开时，依然提示“该操作将关闭连接！是否继续？”的问题。
- 增加了说明文档的软件截图。   

[2.19.1120.2129]
- 重写左侧树形列表样式。
   

   
### 截图效果
- 主页面
![avatar][https://gitee.com/sunnypaine/DeskRedis/blob/master/Screenshot/main.jpg]
   
- 添加连接
![avatar][https://gitee.com/sunnypaine/DeskRedis/blob/master/Screenshot/add.jpg]
   
- 重命名键
![avatar][https://gitee.com/sunnypaine/DeskRedis/blob/master/Screenshot/renamekey.jpg]
   
- 搜索键
![avatar][https://gitee.com/sunnypaine/DeskRedis/blob/master/Screenshot/search.jpg]