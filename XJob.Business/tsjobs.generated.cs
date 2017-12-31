

using System;
using LinqToDB.Mapping;
using FluentValidation;
using GlueNet.Domain.Entities;
using System.ComponentModel;

namespace XJob.Business.Entities
{
   [Serializable]
   [Table("TS_JOBS")]   
   public partial class TsJobs : GlueEntity< TsJobs, string>
    { 
        [Column("c_job_id")]
        public string CJobId { get; set; }
        
        [Column("c_job_desc")]
        public string CJobDesc { get; set; }
        
        [Column("c_job_group")]
        public string CJobGroup { get; set; }
        
        [Column("c_job_status")]
        public string CJobStatus { get; set; }
        
        [Column("c_cron")]
        public string CCron { get; set; }
        
        [Column("d_start_time")]
        public DateTime? DStartTime { get; set; }
         
        [Column("d_next_time")]
        public DateTime? DNextTime { get; set; }
        
        [Column("c_server_name")]
        public string CServerName { get; set; }
        
        [Column("c_ijob_name")]
        public string CIjobName { get; set; }
        
        [Column("c_crt_emp")]
        public string CCrtEmp { get; set; }
        
        [Column("d_crt_date")]
        public DateTime? DCrtDate { get; set; }
        
        [Column("d_Execute_time")]
        public DateTime? DExecuteTime { get; set; }
        
        [Column("d_prev_time")]
        public DateTime? DPrevTime { get; set; }
        
        [Column("n_execute_time")]
        public double? NExecuteTime { get; set; }
        
        [Column("n_execute_num")]
        public double? NExecuteNum { get; set; }
        
    }
}
