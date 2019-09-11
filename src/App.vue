<template>
  <div id="app">
    <PublicNavbar />
    <div class="container-fluid body-content">
      <Banner />
      <Features />
    </div>
  </div>
</template>

<script>
import PublicNavbar from "./components/PublicNavbar";
import Banner from "./components/Banner";
import Features from "./components/Features";

export default {
  name: "app",
  components: {
    PublicNavbar,
    Banner,
    Features
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
