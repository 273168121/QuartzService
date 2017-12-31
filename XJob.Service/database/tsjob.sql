-- Create table
create table TS_JOBS
(
  id             NVARCHAR2(50) not null,
  c_job_id       NVARCHAR2(50),
  c_job_desc     NVARCHAR2(200),
  c_job_group    NVARCHAR2(20),
  c_job_status   NVARCHAR2(20),
  c_cron         NVARCHAR2(200),
  d_start_time   DATE,
  d_end_time     DATE,
  d_next_time    DATE,
  c_server_name  NVARCHAR2(500),
  c_ijob_name    NVARCHAR2(100),
  c_crt_emp      NVARCHAR2(20),
  d_crt_date     DATE,
  d_execute_time DATE,
  d_prev_time    DATE,
  n_execute_time NUMBER,
  n_execute_num  NUMBER
)
tablespace GH_PROD
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
comment on column TS_JOBS.c_job_id
  is '任务名称';
comment on column TS_JOBS.c_job_desc
  is '任务描述';
comment on column TS_JOBS.c_job_group
  is '任务组';
comment on column TS_JOBS.c_job_status
  is '任务状态';
comment on column TS_JOBS.c_cron
  is '执行表达式';
comment on column TS_JOBS.d_start_time
  is '开始时间';
comment on column TS_JOBS.d_end_time
  is '结束时间';
comment on column TS_JOBS.d_next_time
  is '下次执行时间';
comment on column TS_JOBS.c_server_name
  is '服务名';
comment on column TS_JOBS.c_ijob_name
  is '执行方法';
comment on column TS_JOBS.c_crt_emp
  is '创建人';
comment on column TS_JOBS.d_crt_date
  is '创建时间';
comment on column TS_JOBS.d_execute_time
  is '执行时间';
comment on column TS_JOBS.d_prev_time
  is '上次执行时间';
comment on column TS_JOBS.n_execute_time
  is '执行耗时 ms';
comment on column TS_JOBS.n_execute_num
  is '执行次数';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TS_JOBS
  add primary key (ID)
  using index 
  tablespace GH_PROD
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
