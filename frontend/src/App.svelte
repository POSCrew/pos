<script>
  import { Router, Route, Link } from "svelte-navigator";
  import HomePage from "./lib/pages/homepage/HomePage.svelte";
  import LogIn from "./lib/pages/login/LogIn.svelte";
</script>


<Router>
  <Route path="/*">
    <HomePage />
  </Route>
  <Route path="/login">
    <LogIn />
  </Route>
</Router>
