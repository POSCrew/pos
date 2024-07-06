export function daysSinceEpoch(date: Date) {
  return date.getTime() / (1000 * 86400);
}

export function isoDate(date: string) {
  const dateObj = new Date(date);
  return dateObj.getFullYear() +
    "-" +
    String(dateObj.getMonth() + 1).padStart(2, "0") +
    "-" +
    String(dateObj.getDate()).padStart(2, "0");
}
