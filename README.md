# CatLib
Unity3D Framework For XLua

框架正在开发中，改动较为频繁。

文档地址：catlib.io 
PS:已经列入文档的接口不会进行大的调整，尚未列入的可能会进行调整。

项目开发计划：https://www.teambition.com/project/589ce998907a7b661c86de9c/tasks/scrum/589ce9aadf254b9870a7ac90

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
* 网络服务（Http，Socket）（已经完成）
* CPD(catlib package data) 提供一个默认数据包头包体分包方案的组件（已经完成）
* 基础文件服务 （已经完成）
* 多线程服务 （已经完成）
* 时间组件（已经完成）
* 异常处理组件（等待开发）
* Hash组件（已经完成）
* 加密组件（已经完成）
* 集合组件（对于传统字典，hashtable，list的强化）（等待开发）
* 数据验证服务（提供统一的数据验证规范）（等待开发）

扩展组件
* protobuf网络协议组件（等待开发）
* json协议组件（等待开发）
* xml协议组件（等待开发）
* csv文件服务组件（被认领）
* excel文件服务组件（等待开发）
* sqlite 数据库服务组件（等待开发）
* CDO (catlib data object) catlib数据对象服务（提供对csv，excel，sqlite的统一读写接口）（等待开发）
* 本地化组件（等待开发）
* 对象池组件（等待开发）
* 基础音频组件（等待开发）
* 基础UI服务（等待开发）

编辑器拓展
* 多平台自动打包服务（等待开发）
* PSD分层自动布局服务（UGUI ，NGUI）（等待开发）
* 真机Debug强化（等待开发）

## 文件目录描述
* Assets/ 会被打包的目录 （可以通过配置更改）
* Assets/AssetBundle 会被打包成AssetBundle的目录 （可以通过配置更改）
* Assets/NotAssetBundle 不会打包成AssetBundle的目录（直接复制）（可以通过配置更改）

* Release/ 编译打包后生成的目录

* Assets/NotAssetBundle 不会打包成AssetBundle的目录（直接复制）（可以通过配置更改）
* Scripts/ 代码文件目录
* Scripts/Application/ 用户代码目录 (可自定义名字)
* Scripts/System/ 框架文件目录
* Scripts/System/Bootstrap/ 引导程序目录
* Scripts/System/Lib/ 框架组件目录（核心）
* Scripts/System/Lib/Contracts 框架接口文件（重要，因为面向接口开发，里面包含各组件的接口）

## 程序入口
* Scripts/CProgram.cs

## 用户代码入口(默认)
* Scripts/Application/Bootstrap.cs (可自定义，只需配置到config中就可以了)
* Scripts/Application/Config/ 配置文件目录

## 其他
* 交流QQ群：150371044
