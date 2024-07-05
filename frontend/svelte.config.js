import sveltePreprocess from "svelte-preprocess";
import { preprocessMeltUI, sequence } from "@melt-ui/pp";
export default {
  // Consult https://github.com/sveltejs/svelte-preprocess
  // for more information about preprocessors
  preprocess: sequence([preprocessMeltUI(), sveltePreprocess()]),
};
