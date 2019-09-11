<template>
    <div>
        <nav class="navbar fixed-top navbar-expand-lg navbar-dark bg-info">
            <a class="navbar-brand" href="#">ResourceIdea</a>
            <button
                class="navbar-toggler"
                type="button"
                data-toggle="collapse"
                data-target="#publicNavbarContent"
                aria-controls="publicNavbarContent"
                aria-expanded="false"
                aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="publicNavbarContent">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            Home
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#">
                            Features
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#">
                            Pricing
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#">
                            Contact Us
                        </a>
                    </li>
                </ul>
            </div>
        </nav>
    </div>
</template>

<script>
    export default {
        
    }
</script>

<style scoped>
    .navbar-dark .navbar-nav .nav-link {
        color: white;
    }
</style>