<template>
  <div class="relative">
    <TopBar class="center-one-search" :option="topBarOption" :showLocation="showLocation" @clickR='my' >正在排隊  
      
       <div class="count">排隊人數：{{ count }}</div>
       <div class="id">最新 I D：{{ id }}</div>

      <div class='search-pos'>
      <TopSearch @onSearch="onSearch"></TopSearch>
    </div>
    </TopBar>
  
 
    <div class="bg-gray p-58">
      <div class="queue relative">
        <div class="queue-name"><ul>
            <li class="title base-info">
              <span class="number">NO</span>
              <span class="id-info">ID</span>
              <span class="invest-info">金額</span>
              <span class="date-info">時間</span>
            </li>
        </ul></div>
            <ScrollRefresh
      @getData="ToGetBuyDPEList"
      :residualHeight="360"
      :isNeedUp="false"
      class="innerScroll"
    > 
        <div class="listWrap" >
          <ul>
            <li class="content base-info" v-for="item in dataList" :key="item.index">
              <span class="number">{{ item.index + 1 }}</span>
              <span class="id-info">{{ item.id }}</span>
              <span class="invest-info">{{ item.invest }}</span>
              <span class="date-info">{{ item.date }}</span>
            </li>
          </ul>
        </div>
           </ScrollRefresh>
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import ScrollRefresh from "components/ScrollRefresh";
import { http } from 'util/request'
import { GetBuyDPEList } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh
  },
  data() {
    return {
      searchvalue: 0,
      showLocation: true,
      topBarOption: {
        iconLeft: 'back',
        iconRight: 'icon-dingwei'
      },
      count: 0,
      id: 0,
      uid:0,
      dataList: [
        {
          id: 11478,
          index:0,
          invest: '500',
          date: '05:30 17:36:45'
        }
      ]
    }
  },
  mounted() {
  },
  computed: {},
  methods: {
    onSearch(value) {
      this.uid=value
      this.ToGetBuyDPEList()
    },
    my() {
      this.uid=-1
      this.ToGetBuyDPEList()
    },
    ToGetBuyDPEList() {

      http(GetBuyDPEList, { uid: this.uid }, json => {
        if (json.code === 0) {
          this.dataList = json.response
          // console.log(value)
          if(this.uid ==0||!this.uid ){
            this.count = json.response[0].total
            this.id= this.dataList[this.dataList.length-1].id
          }
        }
        else
        {

          if(json.code==10001)
          {
             this.$router.push('/Login')
          }
        }
      })
    }
  },
  created() {
    this.uid = this.$route.query.id||0
     this.ToGetBuyDPEList()
    // if (this.$route.query.id != 'undefined') {
    //   this.ToGetBuyDPEList(this.$route.query.id)
      
    // } else {
    //   this.ToGetBuyDPEList(this.id)
    // }
  }
}
</script>
<style lang="less" scoped>
/deep/.top-bar{
  min-height:500px !important;
}
.search-pos {
  height: 129px;
  width: 100%;
  padding: 0 58px;
  transform: translate(-50%);
   display: block;
}
.count {
  text-align: center;
  color: #fff;
  font-size: 60px;
  width: 100%;
  height: 129px;
  font-weight: 600;
  padding: 0 58px;
  display: block;
  margin-top: -230px;
  margin-bottom: 30px;
  position: relative;
}
.id {
  text-align: center;
  color: #fff;
  font-size: 60px;
  width: 100%;
  height: 129px;
  font-weight: 600;
  padding: 0 58px;
  display: block;
  margin-top: 120px;
  margin-bottom: 30px;
  position: relative;
}
.center-one-search {
  height: 600px;
}
.queue {
  margin-top: -120px;
  border-radius: 40px;
  padding-left: 52px;
  padding-right: 59px;

  padding-bottom: 60px;
  background: #fff;
  border: 2px solid #e5e5e5;
  z-index: 999;
  span{
    text-align: center;
  }
  .queue-name {
    margin-top: 34px;
    margin-bottom: 34px;
    font-size: 60px;
    color: #6e21d1;
  }
  // .listWrap {
  //   height: calc(100vh - 980px);
  //   overflow: scroll;
  // }
  .base-info {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    height: 130px;
    line-height: 100px;
    padding: 21px 49px 21px 48px;
    box-shadow: 0px 4px 2px 0px rgba(0, 0, 0, 0.15);
    border-radius: 21px;
    margin-bottom: 21px;
    .number {
      width: 12%;
    }
    .id-info {
      width: 20%;
    }
    .invest-info {
      width: 18%;
    }
    .date-info {
      width: 50%;
    }
  }
  .title {
    margin-top: 21px;
    background: #e5e5e5;
    font-size: 38px;
    font-weight: bold;
  }
  .content {
    background: #ececec;
    font-size: 38px;
    color: rgba(41, 9, 82, 1);
    opacity: 0.65;
  }
}
</style>
