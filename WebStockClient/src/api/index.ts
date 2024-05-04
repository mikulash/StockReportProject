import ky from "ky";

const api = () =>
  ky.extend({
    timeout: 30000,
    throwHttpErrors: false,
    retry: 0,
    hooks: {
      beforeRequest: [],
      afterResponse: [],
    },
  });

export default api;
