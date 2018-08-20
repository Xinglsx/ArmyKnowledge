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
	   id                   varchar(36)                    not null,
	   loginname            varchar(20)                    null,
	   pwd					varchar(64)                    null,
	   nickname             varchar(32)                    null,
	   profession           varchar(64)                    null,
	   organization         varchar(64)                    null,
	   creditcode           varchar(32)                    null,
	   phonenumber          varchar(11)                    null,
	   avatar               varchar(256)                   null,
	   signatures           varchar(128)                   null,
	   usertype             int                            null,
	   iscertification      int                            null,
	   isadmin              bit                            null,
	   answercount          int                            null,
	   adoptedcount         int                            null,
	   compositescores      int                            null,
	   followcount          int                            null,
	   fanscount            int                            null,
	   userstate            int                            null,
	   registertime			datetime					   null,
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
	   id                   varchar(36)                    not null,
	   userid               varchar(36)                    null,
	   userType				int							   null,
	   realname             varchar(32)                    null,
	   idcardno             varchar(18)                    null,
	   idcardfrontimage     varchar(512)                   null,
	   idcardbackimage      varchar(512)                   null,
	   organization         varchar(64)                    null,
	   creditcode           varchar(32)                    null,
	   creditexpirydate     varchar(10)                    null,
	   othercredite1        varchar(512)                   null,
	   otherexpirydate1     varchar(10)                    null,
	   othercredite2        varchar(512)                   null,
	   otherexpirydate2     varchar(10)                    null,
	   othercredite3        varchar(512)                   null,
	   otherexpirydate3     varchar(10)                    null,
	   certstate            int                            null,
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
	   id                   varchar(36)                    not null,
	   title                varchar(128)                   null,
	   author               varchar(36)                    null,
	   authornickname       varchar(32)                    null,
	   publishtime          datetime                       null,
	   introduction         varchar(512)                   null,
	   content              text                           null,
	   images               varchar(4000)                  null,
	   isrecommend          bit                            null,
	   homeimage            varchar(512)                   null,
	   readcount            int                            null,
	   praisecount          int                            null,
	   commentcount         int                            null,
	   heatcount            int                            null,
	   questionstate        int                            null,
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
	   id                   varchar(36)                    not null,
	   questionid           varchar(36)                    null,
	   userid               varchar(36)                    null,
	   nickname             varchar(32)                    null,
	   publishtime          datetime                       null,
	   content              varchar(8000)                  null,
	   images               varchar(4000)                  null,
	   isadopt              bit                            null,
	   praisecount          int                            null
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
	   id                   varchar(36)                    not null,
	   title                varchar(128)                   null,
	   author               varchar(36)                    null,
	   authornickname       varchar(32)                    null,
	   publishtime          datetime                       null,
	   introduction         varchar(512)                   null,
	   content              text                           null,
	   images               varchar(4000)                  null,
	   isrecommend          bit                            null,
	   homeimage            varchar(512)                   null,
	   readcount            int                            null,
	   heatcount            int                            null,
	   category             varchar(16)                    null,
	   demandstate          int                            null,
	   demandscores         int                            null,
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
	   id					varchar(36)                    not null,
	   userid               varchar(36)                    null,
	   followerusdrid       varchar(36)                    null,
	   followerstate                int                            null
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
	   id                   varchar(36)                    not null,
	   userid1              varchar(36)                    null,
	   nickname1            varchar(32)                    null,
	   userid2              varchar(36)                    null,
	   nickname2            varchar(32)                    null,
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
	   id                   varchar(36)                    not null,
	   msgid                varchar(36)                    null,
	   senduserid           varchar(36)                    null,
	   sendnickname         varchar(32)                    null,
	   sendtime             datetime                       null,
	   content              varchar(512)                   null,
	   contenttype          int                            null
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
	   id					varchar(36)                    not null,
	   title				varchar(256)                   null,
	   noticetype           int                            null,
	   publishtime			datetime                       null,
	   content				text                           null,
	   noticestate          int                            null
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
	   images               varchar(4000)                  null,		--产品图片
	   isrecommend          bit                            null,		--是否推荐
	   homeimage            varchar(512)                   null,		--首页图片
	   publishtime          datetime                       null,		--发布时间
	   compositescore       decimal                        null,		--综合得分
	   materialcode         varchar(32)                    null,		--物资编码
	   productiondate       varchar(10)                    null,		--生产日期
	   prodetail			varchar(2000)				   null,		--产品明细信息
	   category             varchar(64)                    null,		--产品分类
	   contacts             varchar(32)                    null,		--联系人
	   contactphone         varchar(11)                    null,		--联系电话
	   prostate             int                            null,		--产品状态 0-新建 1-提交审核 2-审核通过
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
	   id                   varchar(36)                    not null,
	   userid               varchar(36)                    null,
	   questionid           varchar(36)                    null,
	   lasttime             datetime                       null,
	   iscollect            bit                            null
	   constraint PK_RECORD primary key clustered (id)
	);
end
