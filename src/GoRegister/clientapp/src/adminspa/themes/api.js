import axios from "axios";

const get = () => {
  return new Promise((resolve, reject) => {
    axios
      .get("/api/projecttheme/get", {})
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const save = async (model) => {
  return new Promise((resolve, reject) => {
    axios
      .post("/api/projecttheme/save", model)
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

export {
  get,
  save
};
