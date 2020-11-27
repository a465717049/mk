<template>
    <div style="margin-top: 30px;">
          <div style="margin: 20px;"> 欢迎来到DP后台管理系统!</div>

<el-card class="welcome-card note" style="width: calc(49% - 10px);margin-right: 10px;">
   <div slot="header" class="clearfix">
    <span>操作指南</span>
  </div>
</el-card>
<el-card class="welcome-card"  style="width: 49%;margin: 0;font-size: 14px;">
   <div slot="header" class="clearfix">
    <span style="font-size: 16px;">服务器</span>
   </div>
  <div class="text item">环境变量：{{serverInfo.EnvironmentName}}</div>
  <div class="text item">系统架构：{{serverInfo.OSArchitecture}}</div>
  <div class="text item">ContentRootPath：{{serverInfo.ContentRootPath}}</div>
  <div class="text item">WebRootPath：{{serverInfo.WebRootPath}}</div>
  <div class="text item">.NET Core版本：{{serverInfo.FrameworkDescription}}</div>
  <div class="text item">内存占用：{{serverInfo.MemoryFootprint}}</div>
  <div class="text item">启动时间：{{serverInfo.WorkingTime}}</div>
  <div><br ></div>

    
</el-card>

<el-card class="welcome-card" style="margin-top:20px;width: 98%;">
    <div slot="header" class="clearfix">
        <span>访问日志 <span style="font-size:12px;">(Top 50 desc)</span> </span>
    </div>
<el-table :data="logs" highlight-current-row border
        v-loading="listLoading" 
                  style="width: 100%;font-size: 12px;">
            <el-table-column prop="User" label="访问者" width="150px" sortable>
            </el-table-column>
            <el-table-column prop="IP" label="请求地址" width="150px" >
            </el-table-column>
            <el-table-column prop="BeginTime" label="请求时间" width="150px" >
            </el-table-column>
            <el-table-column prop="API" label="访问接口" width="" >
            </el-table-column>
            <el-table-column prop="RequestMethod" label="Method" width="100px" >
            </el-table-column>
            <el-table-column prop="OPTime" label="响应时长" width="100px" >
            </el-table-column>
            <el-table-column prop="RequestData" label="参数" width="" >
            </el-table-column>
            <el-table-column prop="Agent" label="Agent" width="80" show-overflow-tooltip>
                <template scope="scope">
                    <div style="text-decoration:underline;cursor:pointer;">
                        {{ scope.row.Agent}}
                    </div>
                </template>
            </el-table-column>
          
        </el-table>


    <br>
</el-card>


<el-card class="welcome-card" style="margin-top: 20px;width: 98%;">
    <div slot="header" class="clearfix">
        <span>相关配置</span>
    </div>


    <el-aside>1、动态添加一个vue页面：</el-aside>

    <br>
    
   
    <br>
    <hr>
    <br>




    <br>
</el-card>
    </div>
</template>

<script>
    import applicationUserManager from "../Auth/applicationusermanager";
    import {getServerInfo,getAccessLogs} from '../api/api';
  
  export default {
        name: "Welcome",
         data() {
            return {
                listLoading: false,
                logs: [],
                serverInfo:{}
            }
        },
        mounted() {
            var curTime = new Date()
            if(window.localStorage.TokenExpire){
                var expiretime = new Date(Date.parse(window.localStorage.TokenExpire))
                if(curTime>=expiretime){
                    if (global.IS_IDS4) {
                        applicationUserManager.login();
                    } else {
                        this.$router.push('/login');
                    }
                }
            }else {
                if (global.IS_IDS4) {
                    applicationUserManager.login();
                } else {
                    this.$router.push('/login');
                }
            }

            getServerInfo({}).then((res) => {
                this.serverInfo = res.data.response;
            });

               getAccessLogs({}).then((res) => {
                    this.logs = res.data.response;
                    this.listLoading = false;
                    //NProgress.done();
                });

        },
    }
</script>

<style scoped>
.note .text {
    font-size: 14px;
  }

 .note .item {
    margin-bottom: 18px;
  }
</style>
