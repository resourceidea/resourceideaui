<template>
  <div id="app">
    <PublicNavbar />
    <div class="container-fluid body-content">
      <Banner />
      <Features />
      <ContactUs />
    </div>
  </div>
</template>

<script>
import PublicNavbar from "./components/PublicNavbar";
import Banner from "./components/Banner";
import Features from "./components/Features";
import ContactUs from "./components/ContactUs";

export default {
  name: "app",
  components: {
    PublicNavbar,
    Banner,
    Features,
    ContactUs
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
