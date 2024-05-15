import ky from "ky";

const api = () =>
  ky.extend({
    timeout: 30000,
    throwHttpErrors: true,
    retry: 0,
    hooks: {
      beforeRequest: [],
      afterResponse: [],
    },
  });

export default api;
