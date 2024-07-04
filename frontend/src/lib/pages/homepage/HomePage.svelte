<script lang="ts">
  import { Button, Space } from "ui-commons";
  import { faHouse, faPlus, faUser } from "@fortawesome/free-solid-svg-icons";
  import Fa from "svelte-fa";
  import NavigationMenu from "./NavigationMenu.svelte";
  import MainArea from "./MainArea.svelte";
  import { onMount } from "svelte";
  import { navigate } from "svelte-navigator";
  import { AuthService, tAuthService } from "../../services/AuthService";
  import type { User } from "../../data/User";
  import { sl } from "../../di";
  let authService: AuthService;
  let user: User = null;

  let items = $state([]);
  let invoice = $state({});
  onMount(async () => {
    authService = sl.resolve(tAuthService);
    user = await authService.currentUser();

    checkIfUserLoggedIn();
  });

  function checkIfUserLoggedIn() {
    if (user == null) {
      navigate("/login");
    }
  }
</script>

<div class="navbar bg-slate-200 h-10 flex items-center px-4 md:px-12 lg:px-24 py-6">
  
  {@render navBtn("Sale invoice", faPlus)}
  <Space width="4px"/>
  {@render navBtn("Purchase invoice", faPlus)}


</div>

<main class="flex min-h-full">
  
  <div class="flex-grow-0 w-96">
    <NavigationMenu {items}/>
  </div>
  <div class="grow w-0">
    <MainArea {items}/>
  </div>
</main>


{#snippet navBtn(text, icon)}
<Button hoverColor="#fff3" borderColor='#555' borderThickness=1>
  <Fa icon={icon} /> {text}
</Button>
{/snippet}


<style lang="postcss">
  .navbar{
    @apply border-b-[1px] border-b-slate-600;
  }
</style>