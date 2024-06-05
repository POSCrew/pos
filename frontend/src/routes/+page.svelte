<script lang="ts">
  import { serverUrl } from "$lib/http";
  import { TextField, Button } from "ui-commons";

  let password, email;

  function login(){

  }
</script>

<div class="border m-8"></div>
<h2>{serverUrl}</h2>
<TextField
  type="text"
  label="Username"
  placeholder="Username"
  bind:value={email}
/>

<TextField
  type="password"
  label="Password"
  placeholder="Enter yout password:"
  bind:value={password}
/>
<Button on:click={login}>Login</Button>
