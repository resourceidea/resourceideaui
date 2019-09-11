<template>
  <div id="app">
    <PublicNavbar />
    <div class="container-fluid body-content">
      <Banner />
      <img alt="Vue logo" src="./assets/logo.png" />
      <HelloWorld msg="Welcome to Your Vue.js App" />
    </div>
  </div>
</template>

<script>
import HelloWorld from "./components/HelloWorld.vue";
import PublicNavbar from "./components/PublicNavbar";
import Banner from "./components/Banner";

export default {
  name: "app",
  components: {
    HelloWorld,
    PublicNavbar,
    Banner
  }
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
