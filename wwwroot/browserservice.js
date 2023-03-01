// Browser service
export function ckrd() {
  return (document.cookie.length === 0) ? "z=0" : decodeURIComponent(document.cookie);
}
export function ckwr(name, value, days) {
  let expire;
  if (!Number.isFinite(days))
    expire = "";
  else {
    const date = new Date();
    date.setDate(date.getDate() + days);
    expire = `;expires=${date.toGMTString()}`;
  }
  document.cookie = `${name}=${value}${expire};path=/`;
}
export function getdim() {
  return { width: window.innerWidth, height: window.innerHeight };
}
