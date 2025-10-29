<template>
<tr>
  <td>{{field.label}}</td>
  <td><div v-for="session in sessions" :key="session.id">{{session.name}}</div> </td>
  </tr>
</template>

<script>

export default {
  props: {
    field: {
      type: Object,
      required: true,
    },
    model: {
      type: Object,
      required: true,
    },
  },
  computed: {
    sessions() {
      return this.model[this.field.id].filter(s => !s.isDeleted)//.find((s) => s.id === sessionId).name;
    },
  },
};
</script>