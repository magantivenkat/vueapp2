import axios from "axios";

const getMenu = () => {
  return new Promise((resolve, reject) => {
    axios
      .get("/api/menus/list", {})
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const saveMenu = async (model) => {
  return new Promise((resolve, reject) => {
    axios
      .post("/api/menus/updateList", model)
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const getMenuItemEditModel = (id) => {
  return new Promise((resolve, reject) => {
    axios
      .get("/api/menus/edit-item", {
        params: {
          id,
        },
      })
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const getMenuItemCreateModel = () => {
  return new Promise((resolve, reject) => {
    axios
      .get("/api/menus/create-item", {})
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const createMenuItem = (model) => {
  return new Promise((resolve, reject) => {
    axios
      .post("/api/menus/create-item", model)
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const updateMenuItem = (model) => {
  return new Promise((resolve, reject) => {
    axios
      .put("/api/menus/edit-item", model)
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
};

const deleteMenuItem = (id) => {
  return new Promise((resolve, reject) => {
    axios
      .post("/api/menus/delete-item", id)
      .then(function(response) {
        resolve(response.data);
      })
      .catch(function(error) {
        reject(error);
      });
  });
}

export {
  getMenu,
  saveMenu,
  getMenuItemEditModel,
  getMenuItemCreateModel,
  createMenuItem,
  updateMenuItem,
  deleteMenuItem
};
