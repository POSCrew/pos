import type { AxiosError } from "axios";
import { DialogUtils } from "ui-commons";

export function cssVariables(node, variables) {
  setCssVariables(node, variables);

  return {
    update(variables) {
      setCssVariables(node, variables);
    },
  };
}
function setCssVariables(node, variables) {
  for (const name in variables) {
    node.style.setProperty(`--${name}`, variables[name]);
  }
}

export function dialogErrorHandler(err: AxiosError) {
  DialogUtils.error('', err.response.data.Title || err.response.data.title);
}
