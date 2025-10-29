<template>
  <InputWrapper :context="context" :renderLabel="false">
    <div v-for="category in sessionCategories" :key="category.categoryId">
      <h3>{{ category.categoryName }}</h3>
      <!-- Singleselect Session -->
      <div v-if="category.isSingleSession">
        <div v-for="session in category.sessions" :key="session.id">
          <div
            v-if="
              !value.find((s) => s.id == session.id && !s.isDeleted)
            "
          >
            <div class="card" @click="updateSession(session)">
              <h5 class="card-title">{{ session.name }}</h5>
              <div class="card-body">
                <div>Start Date: {{ session.dateStart }}</div>
                <div>End Date: {{ session.dateEnd }}</div>
                {{ session.description }}
                <button class="btn btn-primary float-right">
                  Select
                </button>
              </div>
            </div>
          </div>
          <div v-else>
            <div class="card active" @click="updateSession(session)">
              <h5 class="card-title">{{ session.name }}</h5>
              <div class="card-body">
                <div>Start Date: {{ session.dateStart }}</div>
                <div>End Date: {{ session.dateEnd }}</div>
                {{ session.description }}
                <button class="btn btn-light float-right">Leave</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Multiselect Session -->
      <div v-else v-for="session in category.sessions" :key="session.id">
        <div
          v-if="!value.find((s) => s.id == session.id && !s.isDeleted)"
        >
          <div class="card" @click="updateSession(session)">
            <h5 class="card-title">{{ session.name }}</h5>
            <div class="card-body">
              <div>Start Date: {{ session.dateStart }}</div>
              <div>End Date: {{ session.dateEnd }}</div>
              {{ session.description }}
              <button class="btn btn-primary float-right">Select</button>
            </div>
          </div>
        </div>
        <div v-else>
          <div class="card active" @click="updateSession(session)">
            <h5 class="card-title">{{ session.name }}</h5>
            <div class="card-body">
              <div>Start Date: {{ session.dateStart }}</div>
              <div>End Date: {{ session.dateEnd }}</div>
              {{ session.description }}
              <button class="btn btn-light float-right">Leave</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </InputWrapper>
</template>

<script>
import inputMixin from "../InputMixin";
import axios from "axios";
import Swal from "sweetalert2";
import InputWrapper from './InputWrapper.vue'

export default {
  components: {InputWrapper},
  mixins: [inputMixin],
  props: {
    sessions: {
      type: Array,
    },
    value: Array,
    reserveSessionUrl: String,
    isAdmin: Boolean,
    model: Object,
  },
  data() {
    return {
      uuid: Math.random().toString(16).substr(2),
    };
  },
  methods: {
    optionSelected(session) {
      return (
        this.value.find(
          (s) => s.categoryId == session.sessionCategoryId && s.id == session.id
        ) !== undefined
      );
    },
    updateValue(key, value) {
      this.$emit("input", value);
    },
    async updateSession(session) {
      const sessionExists = this.value.find((s) => s.id == session.id);
      const sesssionIndex = this.value.indexOf(sessionExists);

      const addDelegateToSession = await axios.post(this.reserveSessionUrl, {
        delegateId: this.model.id,
        sessionId: session.id,
      });

      if (addDelegateToSession.data === "full") {
        // session full - do not add
        Swal.fire({
          title: "This session is now full.",
          text: "Please choose another option.",
          icon: "error",
          confirmButtonText: "Okay",
        });
        return false;
      }
      if (addDelegateToSession.data == "timeclash") {
        // session full - do not add
        Swal.fire({
          title: "You're on another session at this time.",
          text: "Please choose another option.",
          icon: "error",
          confirmButtonText: "Okay",
        });
        return false;
      }
      // Need to fix single session add/remove here
      if (session.isSingleSession) {
        const singleSessionExists = this.value.some(
          (s) => s.categoryId == session.sessionCategoryId
        );
        let newSessions = [...this.value];
        // remove all sessions in selectedSessionList in that category
        if (singleSessionExists) {
          const category = this.value.find(
            (s) => s.categoryId == session.sessionCategoryId
          );
          const categoryIndex = this.value.indexOf(category);
          newSessions.splice(categoryIndex, 1);
        }
        if (addDelegateToSession.data === "added") {
          // clean out all option
          this.updateValue("sessions", [
            ...newSessions,
            {
              id: session.id,
              categoryId: session.sessionCategoryId,
              name: session.name,
              isDeleted: false,
            },
          ]);
        } else {
          this.updateValue("sessions", newSessions);
        }
      } else {
        if (!session.isDeleted && addDelegateToSession.data === "full") {
          if (sessionExists) {
            sessionExists.isDeleted = !sessionExists.isDeleted;

            this.value[sesssionIndex] = sessionExists;

            this.updateValue("sessions", this.value);
          } else {
            this.updateValue("sessions", [
              ...this.value,
              {
                id: session.id,
                categoryId: session.sessionCategoryId,
                name: session.name,
                isDeleted: false,
              },
            ]);
          }
        } else {
          // add/remove session and update model / UI
          if (sessionExists) {
            sessionExists.isDeleted = !sessionExists.isDeleted;

            this.value[sesssionIndex] = sessionExists;

            this.updateValue("sessions", this.value);
          } else {
            this.updateValue("sessions", [
              ...this.value,
              {
                id: session.id,
                categoryId: session.sessionCategoryId,
                name: session.name,
                isDeleted: false,
              },
            ]);
          }
        }
      }
    },
    async updateSingleSession(session, e) {
      const addDelegateToSession = await axios.post(this.reserveSessionUrl, {
        delegateId: this.model.id,
        sessionId: session.id,
      });

      if (addDelegateToSession.data == "full") {
        // session full - do not add
        Swal.fire({
          title: "Oh no! This session is full!",
          text: "Please choose another.",
          icon: "error",
          confirmButtonText: "Cool",
        });
        return false;
      }

      if (addDelegateToSession.data == "timeclash") {
        // session full - do not add
        Swal.fire({
          title: "Oh no! You're on another session!",
          text: "Please choose another.",
          icon: "error",
          confirmButtonText: "Cool",
        });
        return false;
      }

      const singleSessionExists = this.value.some(
        (s) => s.categoryId == session.sessionCategoryId
      );
      let newSessions = [...this.value];
      // remove all sessions in selectedSessionList in that category
      if (singleSessionExists) {
        const category = this.value.find(
          (s) => s.categoryId == session.sessionCategoryId
        );
        const categoryIndex = this.value.indexOf(category);
        newSessions.splice(categoryIndex, 1);
      }
      this.updateValue("sessions", [
        ...newSessions,
        {
          id: session.id,
          categoryId: session.sessionCategoryId,
          name: session.name,
          isDeleted: false,
        },
      ]);
    },
  },
  computed: {
    sessionCategories() {
      // Start by grouping session in their categories
      const sessions = this.sessions;
      const groups = sessions.reduce((groups, session) => {
        if (session.isFull && !this.isAdmin) return groups;
        if (
          !session.regTypeIds.includes(this.model.registrationTypeId) &&
          session.regTypeIds.length > 0
        ) {
          console.log("Showing: ", session.name);
          return groups;
        }

        const group = groups[session.sessionCategoryId] || {
          categoryId: session.sessionCategoryId,
          categoryName: session.sessionCategoryName,
          isSingleSession: session.isSingleSession,
          hasJoined: false,
        };
        group.sessions = group.sessions ? [...group.sessions] : [];
        group.sessions.push(session);
        groups[session.sessionCategoryId] = group;

        return groups;
      }, {});

      return groups;
    },
  },
};
</script>

<style scoped>
label {
  width: 100%;
}

.card-input-element {
  display: none;
}

.card-input {
  padding: 0px;
}

.card-input:hover {
  cursor: pointer;
}

.join-button {
  display: none;
}

.card {
  margin-bottom: 1rem;
}

.card-title {
  border-bottom: 1px solid #dadada;
}

.card-input-element:checked + .card-input {
  box-shadow: 0 0 1px 1px #2ecc71;
}

.card.card.active,
.card-input-element:checked + .card {
  background: #e7edff;
}

.card.card.active .btn {
  background: #f5616f;
  color: white;
}

.card.active .card-title,
.card-input-element:checked + .card .card-title {
  background: #3158c9;
  color: #fff;
}

.card-body {
  padding: 1.25rem;
}
.card-title {
  padding: 1.25rem;
}
</style>