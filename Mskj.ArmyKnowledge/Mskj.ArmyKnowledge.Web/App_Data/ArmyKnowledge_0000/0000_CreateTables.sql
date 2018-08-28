/*==============================================================*/
/* DBMS name :      ArmyKnowledge                               */
/* Created on:     2018/8/15 9:39:59                            */
/* Copyright :     MSKJ                                         */
/*==============================================================*/

/*                           删除所有的表                       */
--drop table answer
--drop table cert
--drop table demand
--drop table fans
--drop table follower
--drop table msg
--drop table msgdetail
--drop table notice
--drop table product 
--drop table question
--drop table record
--drop table users

/*==============================================================*/
/* Table: Users            用户信息                             */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='users' AND type='U')
begin
	create table users 
	(
	   id                   varchar(36)                    not null,	--用户ID，注册时生成
	   loginname            varchar(20)                    null,		--登录名
	   pwd					varchar(64)                    null,		--密码（加密后）
	   nickname             varchar(32)                    null,		--昵称
	   profession           varchar(64)                    null,		--专业（行业）
	   organization         varchar(64)                    null,		--组织（学校、工作单位、企业等）
	   sex					varchar(4)					   null,		--性别
	   area					varchar(128)				   null,		--所在地区
	   position				varchar(32)					   null,		--职位
	   goodpoint			varchar(128)				   null,		--擅长
	   registrationid		varchar(64)					   null,		--极光ID
	   creditcode           varchar(32)                    null,		--信用代码
	   phonenumber          varchar(11)                    null,		--电话号码
	   avatar               varchar(256)                   null,		--头像地址
	   signatures           varchar(128)                   null,		--签名（简介）
	   usertype             int                            null,		--用户类别 
																		--0 注册用户
																		--1 个人认证用户
																		--2 专家认证用户
																		--3 科研院所
																		--4 普通企业用户
																		--5 军企企业用户
																		--9 平台管理员
	   iscertification      int                            null,		--是否认证 
																		--0 未认证
																		--1 已申请认证
	   isadmin              bit                            null,		--2 已通过认证
	   answercount          int                            null,		--回答数
	   adoptedcount         int                            null,		--获赞数
	   compositescores      int                            null,		--综合得分
	   followcount          int                            null,		--粉丝数
	   fanscount            int                            null,		--关注数
	   userstate            int                            null,		--用户状态 0 启用 1停用
	   registertime			datetime					   null,		--注册时间

	   constraint PK_USERS primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: certification            用户认证信息                 */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='cert' AND type='U')
begin
	create table cert 
	(
	   id                   varchar(36)                    not null,	--认证主键ID
	   userid               varchar(36)                    null,		--认证用户ID
	   userType				int							   null,		--用户类别 说明见用户表
	   realname             varchar(32)                    null,		--真实姓名
	   idcardno             varchar(18)                    null,		--身份证号
	   idcardfrontimage     varchar(512)                   null,		--身份证正面图
	   idcardbackimage      varchar(512)                   null,		--身份证背面图
	   organization         varchar(64)                    null,		--组织名称
	   creditcode           varchar(32)                    null,		--社会信用代码
	   creditexpirydate     varchar(10)                    null,		--社会信用代码有效期
	   othercredite1        varchar(512)                   null,		--其他证件1照片
	   otherexpirydate1     varchar(10)                    null,		--其他证件1有奖项
	   othercredite2        varchar(512)                   null,		--其他证件2照片
	   otherexpirydate2     varchar(10)                    null,		--其他证件2有奖项
	   othercredite3        varchar(512)                   null,		--其他证件3照片
	   otherexpirydate3     varchar(10)                    null,		--其他证件4有奖项
	   certstate            int                            null,		--认证信息状态 0-启用 1-停用
	   constraint PK_CERTIFICATION primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: question          问题信息                            */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='question' AND type='U')
begin
	create table question 
	(
	   id                   varchar(36)                    not null,	--问题主键ID
	   title                varchar(128)                   null,		--标题
	   author               varchar(36)                    null,		--作者
	   authornickname       varchar(32)                    null,		--作者昵称
	   publishtime          datetime                       null,		--发布时间
	   introduction         varchar(512)                   null,		--简介
	   content              text                           null,		--内容
	   images               varchar(4000)                  null,		--图片集合，以逗号隔开
	   isrecommend          bit                            null,		--是否推荐
	   homeimage            varchar(512)                   null,		--首页图片
	   readcount            int                            null,		--阅读数
	   praisecount          int                            null,		--点赞数
	   commentcount         int                            null,		--评论数
	   heatcount            int                            null,		--热点指数
	   questionstate        int                            null,		--问题状态 
																		--0 新建
																		--1 提交审核
																		--2 审核通过
	   constraint PK_QUESTION primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: answer           答案信息                             */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='answer' AND type='U')
begin
	create table answer 
	(
	   id                   varchar(36)                    not null,	--答案主键ID
	   questionid           varchar(36)                    null,		--问题ID
	   userid               varchar(36)                    null,		--用户ID
	   nickname             varchar(32)                    null,		--用户昵称
	   publishtime          datetime                       null,		--发布时间
	   content              varchar(8000)                  null,		--内容
	   images               varchar(4000)                  null,		--图片集合，以逗号隔开
	   isadopt              bit                            null,		--是否采纳
	   praisecount          int                            null,		--点赞数量
	   constraint PK_ANSWER primary key clustered (id)
	);
end



/*==============================================================*/
/* Table: demand              需求信息                          */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='demand' AND type='U')
begin
	create table demand 
	(
	   id                   varchar(36)                    not null,	--需求主键ID
	   title                varchar(128)                   null,		--标题
	   author               varchar(36)                    null,		--作者
	   authornickname       varchar(32)                    null,		--作者昵称
	   publishtime          datetime                       null,		--发布时间
	   introduction         varchar(512)                   null,		--简介
	   content              text                           null,		--内容
	   images               varchar(4000)                  null,		--图片集合，以逗号隔开
	   isrecommend          bit                            null,		--是否推荐
	   homeimage            varchar(512)                   null,		--首页图片
	   readcount            int                            null,		--阅读数
	   heatcount            int                            null,		--热点指数
	   category             varchar(16)                    null,		--分类
	   demandstate          int                            null,		--需求状态
	   demandscores         int                            null,		--需求总得分，排名用
	   constraint PK_DEMAND primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: fans                     粉丝列表                     */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='fans' AND type='U')
begin
	create table fans 
	(
	   id						varchar(36)                    not null,	--主键ID
	   userid1					varchar(36)                    null,		--用户ID1
	   userid2			        varchar(36)                    null,		--用户ID2
	   fansstate                int                            null,		--关注状态 
																			--0-互粉 
																			--1-用户2关注用户1
																			--2-用户1关注用户2
	   updatetime				datetime					   null,		--更新时间
	   constraint PK_FANS primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: follower            关注列表（暂不用）                */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='follower' AND type='U')
begin
	create table follower 
	(
	   id					varchar(36)                    not null,	--关系主键ID
	   userid               varchar(36)                    null,		--用户ID
	   followerusdrid       varchar(36)                    null,		--跟随用户ID
	   followerstate                int                    null,		--跟随状态
	   constraint PK_FOLLOWER primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: msg                 私信信息                          */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='msg' AND type='U')
begin
	create table msg 
	(
	   id                   varchar(36)                    not null,	--私信会话ID
	   userid1              varchar(36)                    null,		--用户1ID
	   nickname1            varchar(32)                    null,		--用户1昵称
	   userid2              varchar(36)                    null,		--用户2ID
	   nickname2            varchar(32)                    null,		--用户2昵称
	   updatetime			datetime					   null,		--更新时间
	   constraint PK_MSG primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: msgdetail         私信明细信息                        */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='msgdetail' AND type='U')
begin
	create table msgdetail 
	(
	   id                   varchar(36)                    not null,	--私信明细主键ID
	   msgid                varchar(36)                    null,		--私信会话ID
	   senduserid           varchar(36)                    null,		--发送者ID
	   sendnickname         varchar(32)                    null,		--发送者昵称
	   sendtime             datetime                       null,		--发送时间
	   content              varchar(512)                   null,		--内容
	   contenttype          int                            null,		--内容类别 0-文字
	   constraint PK_MSGDETAIL primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: notice                    通知信息                    */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='notice' AND type='U')
begin
	create table notice 
	(
	   id					varchar(36)                    not null,	--通知主键ID
	   title				varchar(256)                   null,		--标题
	   noticetype           int                            null,		--通知类型
	   publishtime			datetime                       null,		--发布时间
	   content				text                           null,		--内容
	   noticestate          int                            null,		--通知状态 
	   																	--0 新建
																		--1 提交审核
																		--2 审核通过
	   constraint PK_NOTICE primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: product                  产品信息                     */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='product' AND type='U')
begin
	create table product 
	(
	   id                   varchar(36)                    not null,	--主键ID
	   userid               varchar(36)                    null,		--发布用户ID
	   nickname             varchar(32)                    null,		--发布用户昵称
	   proname              varchar(64)                    null,		--产品名称
	   price                varchar(32)                    null,		--价格
	   introduction         varchar(2000)                  null,		--简介
	   images               varchar(4000)                  null,		--图片集合，以逗号隔开
	   isrecommend          bit                            null,		--是否推荐
	   homeimage            varchar(512)                   null,		--首页图片
	   publishtime          datetime                       null,		--发布时间
	   compositescore       decimal                        null,		--综合得分
	   materialcode         varchar(32)                    null,		--物资编码
	   productiondate       varchar(10)                    null,		--生产日期
	   prodetail			text						   null,		--产品明细信息
	   category             varchar(64)                    null,		--产品分类
	   contacts             varchar(32)                    null,		--联系人
	   contactphone         varchar(11)                    null,		--联系电话
	   prostate             int                            null,		--产品状态 
																		--0-新建 
																		--1-提交审核 
																		--2-审核通过
	   proscores			int							   null			--综合得分
	   constraint PK_PRODUCT primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: recode            浏览记录信息                        */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='record' AND type='U')
	begin
	create table record 
	(
	   id                   varchar(36)                    not null,	--浏览记录主键ID
	   userid               varchar(36)                    null,		--用户ID
	   questionid           varchar(36)                    null,		--问题ID
	   lasttime             datetime                       null,		--最后浏览时间
	   iscollect            bit                            null,		--是否收藏
	   constraint PK_RECORD primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: dictionary               字典表                       */
/*==============================================================*/
IF NOT EXISTS( SELECT * FROM sys.objects WHERE name='dictionary' AND type='U')
	begin
	create table dictionary 
	(
	   id                   varchar(36)                    not null,	--字典主键ID
	   dictype              int							   null,		--用户分类
																		--0-专家专业
																		--1-需求类型
																		--2-产品类型
	   diccode				varchar(2)                     null,		--字典Code
	   dicname              varchar(36)                    null,		--字典名称
	   dicstate             bit                            null,		--字典状态
	   constraint PK_DICTIONARY primary key clustered (id)
	);
end
/*插入专家专业字典*/
if not exists (select 1 from dictionary where id = '2302B295-14F3-43FF-AA52-55B4C565F718')
begin
	insert into dictionary select '2302B295-14F3-43FF-AA52-55B4C565F718',0,'01','电子信息技术',1
end
if not exists (select 1 from dictionary where id ='D0C3DBB3-B560-452B-B232-6CEB2CD9AF4E')
begin
	insert into dictionary select 'D0C3DBB3-B560-452B-B232-6CEB2CD9AF4E',0,'02','能源环保技术',1
end
if not exists (select 1 from dictionary where id ='97A3B449-9DEF-4F34-973C-93A6F3E5B90F')
begin
	insert into dictionary select '97A3B449-9DEF-4F34-973C-93A6F3E5B90F',0,'03','新材料技术',1
end
if not exists (select 1 from dictionary where id ='DE6D57D2-19AA-4F10-8A50-8289F0C20C63')
begin
	insert into dictionary select 'DE6D57D2-19AA-4F10-8A50-8289F0C20C63',0,'04','航空航天技术',1
end
if not exists (select 1 from dictionary where id ='6CCA67BA-10B7-47E7-A4B3-16ECA796E9DE')
begin
	insert into dictionary select '6CCA67BA-10B7-47E7-A4B3-16ECA796E9DE',0,'05','光机电一休化',1
end
if not exists (select 1 from dictionary where id ='2B8E58AE-B8A2-458E-A1A0-01A4C6442872')
begin
	insert into dictionary select '2B8E58AE-B8A2-458E-A1A0-01A4C6442872',0,'06','生物医药技术',1
end
/*插入产品类型字典*/
if not exists (select 1 from dictionary where id ='049D0A85-3514-4940-9417-FCAF99405880')
begin
	insert into dictionary select '049D0A85-3514-4940-9417-FCAF99405880',2,'01','元器件',1
end
if not exists (select 1 from dictionary where id ='A4D7377B-AB1B-4919-B9AC-AB590FB82BC7')
begin
	insert into dictionary select 'A4D7377B-AB1B-4919-B9AC-AB590FB82BC7',2,'02','电力装备',1
end
if not exists (select 1 from dictionary where id ='E34E8614-C6EF-4E0B-B89A-BC5BBF30DDAE')
begin
	insert into dictionary select 'E34E8614-C6EF-4E0B-B89A-BC5BBF30DDAE',2,'03','工业基础件',1
end
if not exists (select 1 from dictionary where id ='AFEBD3E9-5BCA-4413-94A8-AF48042221A8')
begin
	insert into dictionary select 'AFEBD3E9-5BCA-4413-94A8-AF48042221A8',2,'04','应急救援保障',1
end
if not exists (select 1 from dictionary where id ='AE69E62E-876B-4345-B507-587674BA0A1A')
begin
	insert into dictionary select 'AE69E62E-876B-4345-B507-587674BA0A1A',2,'05','新材料',1
end
