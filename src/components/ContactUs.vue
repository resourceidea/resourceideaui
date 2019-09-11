<template>
  <div>
    <div class="row section-container">
      <div class="col-sm-12">
        <h1 class="display-2 text-center contactus-section-header">
          <a name="contactus">Contact Us</a>
        </h1>
      </div>
      <form class="offset-sm-3 col-sm-6">
        <div class="card bg-info text-white">
          <div class="card-header bg-light text-info">
            <strong
              class="lead"
              style="font-weight:bold"
            >Please contact us for a quote. We love to hear from you!</strong>
          </div>
          <div class="card-body">
            <div class="form-row">
              <div class="form-group col-sm-6">
                <label for="inputEmail4">Email</label>
                <input type="email" class="form-control" id="inputEmail4" placeholder="Email" />
              </div>
              <div class="form-group col-sm-6">
                <label for="inputPassword4">Fullname</label>
                <input type="text" class="form-control" id="inputPassword4" placeholder="Your name" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group col-sm-12">
                <label for="inputEmail4">Message</label>
                <textarea class="form-control" placeholder="Message"></textarea>
              </div>
            </div>
          </div>
          <div class="card-footer text-muted">
            <input type="submit" class="btn btn-danger" value="Send message" />
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style scoped>
.section-container {
  background-color: #f9f9f9;
  padding: 40px 0 40px 0;
}
.contactus-section-header {
  color: #404040;
}
.contactus-sub-title {
  color: #0092a9;
}
</style>