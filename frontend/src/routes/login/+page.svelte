<script lang="ts">
  import { goto } from "$app/navigation";
  import { Http, serverUrl } from "$lib/http";
  import { TextField, Button, Space, DialogUtils } from "ui-commons";

  const http = Http.get();
  let password, username;

  function login() {
    let loginReq = { password, username };
    console.log();
    http.post("/users/login", loginReq).then((res) => {
      if (res.status === 200){
        goto("/")
      }
    });
  }
  function createAdmin() {
    http.post("/users/registerAdmin", null).then((res) => {
      DialogUtils.message(`Username: admin,  Password: ${res.data.Password}`);
    });
  }
</script>

<div class="border m-auto my-12 lg:w-[50%] w-[75%] py-14 px-14 lg:px-36">
  <Button on:click={createAdmin} borderColor="gray" borderThickness="1"
    >Create Admin User</Button
  >

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
