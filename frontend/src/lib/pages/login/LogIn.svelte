<script lang="ts">
  import { TextField, Button, Space, DialogUtils } from "ui-commons";
  import { sl } from "../../di";
  import { AuthService, tAuthService } from "../../services/AuthService";
  import { navigate } from "svelte-navigator";
  import {
    tStoreService,
    type StoreService,
  } from "../../services/StoreService";
  import { onMount } from "svelte";
  let service: AuthService = sl.resolve(tAuthService);
  let storeService: StoreService = sl.resolve(tStoreService);
  let password = $state(""),
    username = $state("");

  let storeTitle = $state("");
  let storeAddress = $state("");
  let isStoreInitialized = $state(false);
  onMount(async () => {
    isStoreInitialized = await storeService.isStoreInitialized();
    if(isStoreInitialized){
      storeTitle = (await storeService.getStore()).title
    }
  });
  function login() {
    let loginReq = { password, username };
    console.log();
    service.login(loginReq).then((res) => {
      navigate("/");
    });
  }
  function createAdmin() {
    service.createAdmin().then((res) => {
      DialogUtils.message(`Username: admin,  Password: ${res.data.Password}`);
    });
  }
  function initStore() {
    storeService
      .initializeStore({ title: storeTitle, address: storeAddress })
      .then((res) => {
        storeTitle = res.title;
        storeAddress = res.address;
        DialogUtils.message(`${res.title}\nAddress: ${res.address}`);
      });
  }
</script>

<div class="border m-auto my-12 lg:w-[50%] w-[75%] py-14 px-14 lg:px-36">
  {#if isStoreInitialized}
    <div>
      <h2 class="text-gray-500">Store title: <span class="text-gray-800">{storeTitle}</span> </h2>
      <div class="h-1"></div>
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
      <Button on:click={login} borderColor="gray" borderThickness="1"
        >Login</Button
      >
    </div>
  {:else}
    <p class="text-center">You have to initialize store in order to use it.</p>
    <TextField
      type="text"
      label="Store title"
      placeholder="Title"
      bind:value={storeTitle}
    />

    <TextField
      type="text"
      label="Address"
      placeholder="Enter store address"
      bind:value={storeAddress}
    />
    <div class="h-2"></div>

    <Button on:click={initStore} borderColor="gray" borderThickness="1"
      >Initialize Store</Button
    >
  {/if}
</div>
