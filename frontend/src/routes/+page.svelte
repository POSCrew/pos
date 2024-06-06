<script lang="ts">
  import { Http, serverUrl } from "$lib/http";
  import { TextField, Button, Space } from "ui-commons";

  const http = Http.get();
  let password, username;

  function login() {
    let loginReq = { password, username };
    console.log();
    http.post("/users/login", loginReq).then((res) => {
      console.log(res);
    });
  }
</script>

<div class="border m-auto my-12 w-[50%] py-14 px-14 md:px-36">
  <TextField
    type="text"
    label="Username"
    placeholder="Username"
    bind:value={username}
  />

  <TextField
    type="password"
    label="Password"
    placeholder="Enter yout password:"
    bind:value={password}
  />
  <div class="h-2"></div>
  <Button on:click={login} borderColor="gray" borderThickness="1">Login</Button>
</div>
