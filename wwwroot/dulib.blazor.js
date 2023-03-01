// DULIB.BLAZOR
DUBZ = {
  ckrd: function () {
    return (document.cookie.length === 0) ? 'z=0' : decodeURIComponent(document.cookie);
  },
  ckwr: function (name, value, days) {
    document.cookie = `${name}=${value}${this.ckexr(days)};path=/`;
  },
  ckrm: function (name) {
    document.cookie = `${name}=;path=/;expires=${(new Date()).toGMTString()}`;
  },
  ckexr: function (days) {
    if (!Number.isFinite(days))
      return "";
    return `;expires=${this.ckafs(days).toGMTString()}`;
  },
  ckafs: function (days) {
    const date = new Date();
    date.setDate(date.getDate() + days);
    return date;
  },
  getdim: function () {
    return { width: window.innerWidth, height: window.innerHeight };
  }
}
