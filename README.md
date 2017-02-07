# CatLib
Unity3D Framework For XLua

框架正在开发中，改动较为频繁。

欢迎大家认领组件开发，一起完善框架~！

## 计划的功能

框架核心
* 容器服务 （已经完成）
* 接口服务 （正在制定）
* 配置服务 （已经完成）

基础组件
* 基础自动更新服务 （已经完成）
* AssetBundle资源服务（已经完成）
* 事件服务（已经完成）
* XLua热更新支持 （正在开发）
* 网络服务（Http，Socket）（正在开发）
* 基础文件服务 （正在开发）
* 多线程服务 （被认领）
* 基础UI服务（等待开发）
* 异常处理组件（等待开发）
* 对象池组件（等待开发）
* 基础音频组件（等待开发）

扩展组件
* protobuf网络协议组件（等待开发）
* json协议组件（等待开发）
* xml协议组件（等待开发）
* csv文件服务组件（等待开发）
* excel文件服务组件（等待开发）
* 本地化组件（等待开发）

编辑器拓展
* 多平台自动打包服务（等待开发）
* PSD分层自动布局服务（UGUI ，NGUI）（等待开发）

## 文件目录描述
* Scripts/ 代码文件目录
* Scripts/Application/ 用户代码目录 (可自定义名字)
* Scripts/Config/ 框架组件配置目录
* Scripts/System/ 框架文件目录
* Scripts/System/Bootstrap/ 引导程序目录
* Scripts/System/Lib/ 框架组件目录（核心）
* Scripts/System/Lib/Contracts 框架接口文件（重要，因为面向接口开发，里面包含各组件的接口）

## 程序入口
* Scripts/CProgram.cs

## 用户代码入口(默认)
* Scripts/Application/CBootstrapProvider.cs (可自定义，只需配置到config中就可以了)

