<template>
  <div>
    <div class="row" id="banner">
      <div class="banner-container text-left">
        <h1 class="display-1" style="margin-top: 220px;">
          <strong>ResourceIdea</strong>
        </h1>
        <p
          class="lead"
          style="font-size: 40px"
        >Your perfect tool for planning and managing your team's resources, clients and jobs.</p>
        <div class="col-sm-5">
          <form asp-action="login" asp-controller="account" method="post">
            <div class="card bg-light border-info">
              <div class="card-header bg-info text-white">
                <span class="card-title">
                  <strong>Login</strong>
                </span>
              </div>
              <div class="card-body">
                <div class="form-row">
                  <div class="form-group col-sm-6">
                    <small>Email</small>
                    <input
                      type="email"
                      class="form-control form-control-sm"
                      asp-for="Email"
                      placeholder="Email"
                    />
                    <span asp-validation-for="Email" class="text-danger"></span>
                  </div>
                  <div class="form-group col-sm-6">
                    <small>Password</small>
                    <input
                      type="password"
                      class="form-control form-control-sm"
                      asp-for="Password"
                      placeholder="Password"
                    />
                    <span asp-validation-for="Password" class="text-danger"></span>
                  </div>
                </div>
                <button type="submit" class="btn btn-primary">Login</button>
                <a href="#signup" class="btn btn-link">Sign up to create an account &raquo;</a>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style scoped>
#banner {
  background: url(/assets/banner5.jpg) no-repeat center center fixed;
  margin-top: -270px;
  background-size: cover;
  -webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  min-height: 680px;
}
.banner-container {
  background-color: rgba(255, 255, 255, 0.8);
  color: black;
  padding: 40px 40px 40px 40px;
}
</style>