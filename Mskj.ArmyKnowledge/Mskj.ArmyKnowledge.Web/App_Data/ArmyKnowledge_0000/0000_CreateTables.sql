/*==============================================================*/
/* DBMS name :      ArmyKnowledge                               */
/* Created on:     2018/8/15 9:39:59                            */
/* Copyright :     MSKJ                                         */
/*==============================================================*/

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
	   isaccept             bit                            null,
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
	   id						varchar(36)                    not null,
	   userid					varchar(36)                    null,
	   fansuserid		        varchar(36)                    null,
	   fansstate                int                            null
	   constraint PK_FANS primary key clustered (id)
	);
end

/*==============================================================*/
/* Table: follower                 关注列表                     */
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
	   id                   varchar(36)                    not null,
	   proname              varchar(64)                    null,
	   price                varchar(32)                    null,
	   introduction         varchar(2000)                  null,
	   protype              varchar(32)                    null,
	   publishtime          datetime                       null,
	   compositescore       decimal                        null,
	   materialcode         varchar(32)                    null,
	   productiondate       varchar(10)                    null,
	   prodetail			varchar(2000)				   null,
	   category             varchar(64)                    null,
	   contacts             varchar(32)                    null,
	   contactphone         varchar(11)                    null,
	   prostate             int                            null
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
